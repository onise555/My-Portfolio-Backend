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
            var accessKey = config["AWS_ACCESS_KEY_ID"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"];
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? "modular-briefcase";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = "https://t3.storage.dev",
                ForcePathStyle = false,
                UseHttp = false
            });
        }

        // აი აქ დავამატე string folder, რომ წითელი ხაზი გაქრეს
        public async Task<string?> UploadFileAsync(IFormFile? file, string folder)
        {
            if (file == null || file.Length == 0) return null;

            // ფაილის სახელი: ფოლდერი + უნიკალური ID
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead,
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(putRequest);

            // აბრუნებს მუშა ლინკს: modular-briefcase.t3.storage.dev/users/images/...
            return $"https://{_bucketName}.t3.storage.dev/{fileKey}";
        }
    }
}