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

            var accessKey = config["AWS_ACCESS_KEY_ID"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"];
            var bucketName = config["AWS_S3_BUCKET_NAME"] ?? "modular-briefcase";
            var serviceUrl = "https://t3.storage.dev";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                // t3.storage.dev-სთვის სცადე true, თუ წინა ვერსიაზე false-მა არ იმუშავა
                ForcePathStyle = true,
                UseHttp = false
            };

            using var client = new AmazonS3Client(credentials, s3Config);

            // ფაილის გასაღები: ახლა folder-საც ვიყენებთ, რომ სერვერზე ფაილები დალაგებული იყოს
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileKey,
                BucketName = bucketName,
                ContentType = file.ContentType,
                // CannedACL ამოღებულია Access Denied-ის თავიდან ასაცილებლად
                AutoCloseStream = false
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            // აბრუნებს სრულ ლინკს
            return $"https://{bucketName}.t3.storage.dev/{fileKey}";
        }
    }
}