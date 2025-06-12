using System;
using System.Data;
using Npgsql;
using BiomechanicNetwork.Utilities;
using BiomechanicNetwork.Database;
using BiomechanicNetwork.Models;

namespace BiomechanicNetwork.Repositories
{
    internal class UserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UserRepository()
        {
            _dbHelper = new DatabaseHelper();
        }
        public UserModel GetUserById(int userId)
        {
            var query = @"SELECT id, name, avatar_public_id, role_id FROM users WHERE id = @userId";

            var parameters = new NpgsqlParameter[]
            {
        new NpgsqlParameter("@userId", userId)
            };

            using (var dt = _dbHelper.ExecuteQuery(query, parameters))
            {
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return new UserModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        AvatarPublicId = row["avatar_public_id"]?.ToString(),
                        RoleId = Convert.ToInt32(row["role_id"])
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        public bool Register(string username, string password, string name, int roleId = 1)
        {
            // Проверяем, существует ли пользователь с таким именем
            if (UserExists(username))
            {
                return false;
            }

            // Генерируем хэш и соль для пароля
            var (hash, salt) = PasswordHelper.GenerateHash(password);

            // SQL запрос для вставки нового пользователя
            var query = @"INSERT INTO users (username, password_hash, salt, name, role_id) 
                          VALUES (@username, @passwordHash, @salt, @name, @roleId)";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@passwordHash", hash),
                new NpgsqlParameter("@salt", salt),
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@roleId", roleId)
            };

            // Выполняем запрос и возвращаем результат
            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Аутентифицирует пользователя
        /// </summary>
        public (bool Success, int UserId, int RoleId) Authenticate(string username, string password)
        {
            // Получаем данные пользователя из базы
            var query = @"SELECT id, password_hash, salt, role_id FROM users WHERE username = @username";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@username", username) };

            DataTable result = _dbHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 0)
            {
                return (false, 0, 0); // Пользователь не найден
            }

            DataRow row = result.Rows[0];
            string storedHash = row["password_hash"].ToString();
            string storedSalt = row["salt"].ToString();
            int userId = (int)row["id"];
            int roleId = (int)row["role_id"];

            // Проверяем пароль
            bool passwordValid = PasswordHelper.VerifyPassword(password, storedHash, storedSalt);


            return (passwordValid, passwordValid ? userId : 0, passwordValid ? roleId : 0);
        }

        /// <summary>
        /// Проверяет существование пользователя
        /// </summary>
        public bool UserExists(string username)
        {
            var query = "SELECT COUNT(1) FROM users WHERE username = @username";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@username", username) };

            int count = _dbHelper.ExecuteScalar<int>(query, parameters);
            return count > 0;
        }

        /// <summary>
        /// Изменяет пароль пользователя
        /// </summary>
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            // Сначала проверяем текущий пароль
            var query = "SELECT password_hash, salt FROM users WHERE id = @userId";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@userId", userId) };

            DataTable result = _dbHelper.ExecuteQuery(query, parameters);

            if (result.Rows.Count == 0)
            {
                return false; // Пользователь не найден
            }

            DataRow row = result.Rows[0];
            string storedHash = row["password_hash"].ToString();
            string storedSalt = row["salt"].ToString();

            if (!PasswordHelper.VerifyPassword(currentPassword, storedHash, storedSalt))
            {
                return false; // Текущий пароль неверный
            }

            // Генерируем новый хэш и соль
            var (newHash, newSalt) = PasswordHelper.GenerateHash(newPassword);

            // Обновляем пароль
            query = "UPDATE users SET password_hash = @newHash, salt = @newSalt WHERE id = @userId";
            parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@newHash", newHash),
                new NpgsqlParameter("@newSalt", newSalt),
                new NpgsqlParameter("@userId", userId)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

      
    }
}