using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

namespace Portfolio.Asp.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _serviceUrl;

        public S3Service(IConfiguration config)
        {
            // Railway-ს ცვლადების პრიორიტეტი. 
            // დარწმუნდი, რომ Railway-ზე AWS_S3_BUCKET_NAME არის "images-55"
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "";

            // ვიყენებთ storage.dev-ს, რადგან შენს მუშა ლინკში ასეა
            _serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storage.dev";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                ForcePathStyle = false,
                UseHttp = false,
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folder = "users/images")
        {
            if (file == null || file.Length == 0) return null;

            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var contentType = GetSafeContentType(file.FileName, file.ContentType);

            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(putRequest);

            // ლინკის აწყობა ზუსტად იმ ფორმატში, რაც შენთან მუშაობს:
            // https://images-55.t3.storage.dev/users/images/სახელი.png
            var cleanUrl = _serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{_bucketName}.{cleanUrl}/{fileKey}";
        }

        private static string GetSafeContentType(string fileName, string originalContentType)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".mp4" => "video/mp4",
                _ => originalContentType ?? "application/octet-stream"
            };
        }
    }
}