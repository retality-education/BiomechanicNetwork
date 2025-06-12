using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BiomechanicNetwork.Utilities
{
    internal static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 бит
        private const int HashSize = 32; // 256 бит
        private const int Iterations = 10000; // Количество итераций PBKDF2

        /// <summary>
        /// Генерирует хэш пароля и соль
        /// </summary>
        /// <param name="password">Пароль в чистом виде</param>
        /// <returns>Кортеж (хэш, соль) в формате base64</returns>
        public static (string Hash, string Salt) GenerateHash(string password)
        {
            // Генерация случайной соли
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Создание хэша с помощью PBKDF2
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password: password,
                salt: salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            return (
                Hash: Convert.ToBase64String(hash),
                Salt: Convert.ToBase64String(salt)
            );
        }

        /// <summary>
        /// Проверяет пароль на соответствие хэшу
        /// </summary>
        /// <param name="password">Пароль в чистом виде</param>
        /// <param name="storedHash">Хранимый хэш (base64)</param>
        /// <param name="storedSalt">Хранимая соль (base64)</param>
        /// <returns>True если пароль верный</returns>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                byte[] salt = Convert.FromBase64String(storedSalt);
                byte[] hash = Convert.FromBase64String(storedHash);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password: password,
                    salt: salt,
                    iterations: Iterations,
                    hashAlgorithm: HashAlgorithmName.SHA256))
                {
                    byte[] testHash = pbkdf2.GetBytes(HashSize);

                    // Сравнение хэшей с постоянным временем выполнения
                    return CryptographicOperations.FixedTimeEquals(testHash, hash);
                }
            }
            catch
            {
                return false; // В случае ошибки формата данных
            }
        }

        /// <summary>
        /// Генерирует случайный пароль заданной длины
        /// </summary>
        /// <param name="length">Длина пароля (по умолчанию 12)</param>
        /// <returns>Случайный пароль</returns>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(chars);
        }
    }
}
