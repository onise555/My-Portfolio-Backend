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

            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "coordinated-pocket-nxuvrv";
            var serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storageapi.dev";

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("AWS credentials missing!");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            using var client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                UseHttp = false
            });

            var extension = Path.GetExtension(file.FileName);
            var fileKey = $"{folder}/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            await client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            });

            // დაბრუნებული ლინკი
            return $"https://{bucketName}.up.railway.app/{fileKey}";
        }
    }
}