using BiomechanicNetwork.Utilities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Drawing;
using System.IO;

namespace BiomechanicNetwork.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            try
            {
                var account = new Account(
                    SecretsManager.GetCloudinaryCloudName(),
                    SecretsManager.GetCloudinaryApiKey(),
                    SecretsManager.GetCloudinaryApiSecret());

                _cloudinary = new Cloudinary(account);
                _cloudinary.Api.Secure = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка инициализации Cloudinary. Проверьте настройки в .env файле", ex);
            }
        }

        public string UploadVideo(string filePath, string publicId = null)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден", filePath);

            if (_cloudinary == null)
                throw new InvalidOperationException("Cloudinary не инициализирован");

            try
            {
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(@"test.mp4"),
                    PublicId = "dog_closeup",
                };

                var uploadResult = _cloudinary.Upload(uploadParams);

                if (uploadResult == null)
                    throw new Exception("Cloudinary вернул null");

                return uploadResult.PublicId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки видео: {ex.Message}", ex);
            }
        }

        public Image GetImage(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return null;

            try
            {
                var url = _cloudinary.Api.UrlImgUp
                    .Transform(new Transformation().Quality("auto"))
                    .BuildUrl(publicId);

                using (var webClient = new System.Net.WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(url);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        return Image.FromStream(ms);
                    }
                }
            }
            catch
            {
                // Логируем ошибку (можно добавить ILogger)
                return null;
            }
        }
    }
}