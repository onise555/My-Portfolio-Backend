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
            // Railway-ს ცვლადების პრიორიტეტი ავტომატური დაკავშირებისთვის
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "";
            _serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                ForcePathStyle = false, // აუცილებელია Virtual-hosted URL-ისთვის
                UseHttp = false,
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folder = "portfolio")
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
                CannedACL = S3CannedACL.PublicRead, // აუცილებელია საჯარო წვდომისთვის
                DisablePayloadSigning = true
            };

            // ატვირთვის პროცესი
            await _s3Client.PutObjectAsync(putRequest);

            // ლინკის სწორი ფორმატი: https://bucket-name.endpoint/key
            var cleanServiceUrl = _serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{_bucketName}.{cleanServiceUrl}/{fileKey}";
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
                _ => originalContentType ?? "application/octet-stream"
            };
        }
    }
}