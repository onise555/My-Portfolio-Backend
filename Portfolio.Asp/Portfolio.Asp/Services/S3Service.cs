using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace Portfolio.Asp.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration config)
        {
            // 1. მონაცემების წაკითხვა (Railway Variables -> AppSettings)
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];

            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "coordinated-pocket-nxuvrv";
            var serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storageapi.dev";
            var region = config["AWS_DEFAULT_REGION"] ?? config["S3Config:Region"] ?? "auto";

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("S3 Credentials missing! Check Railway Variables.");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                AuthenticationRegion = region
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folder)
        {
            if (file == null || file.Length == 0) return null;

            var extension = Path.GetExtension(file.FileName);
            var fileKey = $"{folder}/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            try
            {
                await _s3Client.PutObjectAsync(putRequest);
                // დაბრუნებული ლინკი, რომელიც საჯაროდ იხსნება Railway-ზე
                return $"https://{_bucketName}.up.railway.app/{fileKey}";
            }
            catch (Exception ex)
            {
                throw new Exception($"S3 Upload failed: {ex.Message}");
            }
        }
    }
}