using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;

namespace Portfolio.Asp.FileUploader
{
    public static class FileUploadHelper
    {
        public static async Task<string?> UploadImg(IFormFile? file, string folder, IConfiguration config)
        {
            if (file == null || file.Length == 0) return null;

            // Railway-ს ცვლადების პრიორიტეტი (ავტომატური დაკავშირებისთვის)
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            var serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "";
            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"];

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                // Virtual-hosted URL-ისთვის აუცილებელია false
                ForcePathStyle = false,
                UseHttp = false
            };

            using var client = new AmazonS3Client(credentials, s3Config);

            // უნიკალური სახელი ფაილისთვის
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var contentType = GetSafeContentType(file.FileName, file.ContentType);

            using var stream = file.OpenReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileKey,
                BucketName = bucketName,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead, // საჯარო წვდომისთვის
                AutoCloseStream = false
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            // ვაწყობთ საბოლოო ლინკს: https://bucket.service/key
            var host = serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{bucketName}.{host}/{fileKey}";
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