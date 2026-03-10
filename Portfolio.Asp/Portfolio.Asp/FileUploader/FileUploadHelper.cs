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

            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            var serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storage.dev";
            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"];

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = false,
                UseHttp = false
            };

            using var client = new AmazonS3Client(credentials, s3Config);
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileKey,
                BucketName = bucketName,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead, // აუცილებელია, რომ XML Error აღარ ამოაგდოს
                AutoCloseStream = false
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            var host = serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{bucketName}.{host}/{fileKey}";
        }
    }
}