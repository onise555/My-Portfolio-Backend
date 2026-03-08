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

            var accessKey = config["S3Config:AccessKey"];
            var secretKey = config["S3Config:SecretKey"];
            var serviceUrl = config["S3Config:ServiceUrl"] ?? "";
            var bucketName = config["S3Config:BucketName"];

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true,
                UseHttp = false
            };

            using var client = new AmazonS3Client(credentials, s3Config);

            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // ContentType - safe მნიშვნელობა extension-ის მიხედვით
            var contentType = GetSafeContentType(file.FileName, file.ContentType);

            using var stream = file.OpenReadStream();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileKey,
                BucketName = bucketName,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,
                AutoCloseStream = false
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"{serviceUrl.TrimEnd('/')}/{bucketName}/{fileKey}";
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