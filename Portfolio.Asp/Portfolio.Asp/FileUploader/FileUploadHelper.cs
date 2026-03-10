using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;

namespace Portfolio.Asp.FileUploader
{
    public static class FileUploadHelper
    {
        // დავამატეთ string folder პარამეტრი, რომ UserService-თან თავსებადი იყოს
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
                ForcePathStyle = false, // აუცილებელია Virtual-hosted სტილისთვის
                UseHttp = false
            };

            using var client = new AmazonS3Client(credentials, s3Config);

            // ფაილის გასაღები (Key). თუ გინდა ფოლდერში ჩაჯდეს: $"{folder}/{Guid.NewGuid()}..."
            // მაგრამ რადგან შენი მუშა ლინკი პირდაპირია, გამოვიყენოთ პირდაპირი სახელი:
            var fileKey = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileKey,
                BucketName = bucketName,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead,
                AutoCloseStream = false
            };

            var transferUtility = new TransferUtility(client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"https://{bucketName}.t3.storage.dev/{fileKey}";
        }
    }
}