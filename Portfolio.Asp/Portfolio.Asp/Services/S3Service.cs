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
            var accessKey = config["S3Config:AccessKey"];
            var secretKey = config["S3Config:SecretKey"];
            _bucketName = config["S3Config:BucketName"] ?? "";
            _serviceUrl = config["S3Config:ServiceUrl"] ?? "";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                ForcePathStyle = true,
                UseHttp = false,
                SignatureVersion = "4"
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
                CannedACL = S3CannedACL.PublicRead,
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(putRequest);

            return $"{_serviceUrl.TrimEnd('/')}/{_bucketName}/{fileKey}";
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
                ".mov" => "video/quicktime",
                ".avi" => "video/x-msvideo",
                ".mkv" => "video/x-matroska",
                ".webm" => "video/webm",
                _ => originalContentType ?? "application/octet-stream"
            };
        }
    }
}