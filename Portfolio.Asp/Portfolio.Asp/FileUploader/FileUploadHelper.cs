using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace Portfolio.Asp.FileUploader
{
    public static class FileUploadHelper
    {
        public static async Task<string?> UploadImg(IFormFile? file, string folder, IConfiguration config)
        {
            if (file == null || file.Length == 0) return null;

            var accessKey = config["AWS_ACCESS_KEY_ID"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"];

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("AWS credentials missing!");

            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? "modular-briefcase";
            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            using var client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = "https://t3.storage.dev",
                ForcePathStyle = true,
                UseHttp = false
            });

            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();

            await client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            });

            return $"https://{bucketName}.t3.storage.dev/{fileKey}";
        }
    }
}