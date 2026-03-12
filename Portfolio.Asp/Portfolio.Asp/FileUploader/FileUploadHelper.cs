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

            // 1. მონაცემების წაკითხვა Fallback-ით (Railway Variables -> AppSettings)
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "coordinated-pocket-nxuvrv";
            var serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storageapi.dev";

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("AWS credentials missing! Check Railway environment variables.");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            // 2. კლიენტის შექმნა
            using var client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                UseHttp = false
            });

            // 3. ფაილის მომზადება
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            // 4. ატვირთვა
            await client.PutObjectAsync(putRequest);

            // 5. სწორი URL-ის დაბრუნება (t3.storageapi.dev-ის გათვალისწინებით)
            var baseUrl = serviceUrl.TrimEnd('/');
            return $"{baseUrl}/{bucketName}/{fileKey}";
        }
    }
}