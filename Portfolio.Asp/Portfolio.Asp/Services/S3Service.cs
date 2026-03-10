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
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];

            // ვიყენებთ modular-briefcase ბაქეტს
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "modular-briefcase";

            // ვიყენებთ ზუსტად storage.dev-ს
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

        public async Task<string?> UploadFileAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            // ფაილი იტვირთება პირდაპირ სახელით, ფოლდერების გარეშე
            var fileKey = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead, // აუცილებელია საჯაროობისთვის
                DisablePayloadSigning = true
            };

            await _s3Client.PutObjectAsync(putRequest);

            // აწყობს ლინკს: https://modular-briefcase.t3.storage.dev/filename.ext
            var host = _serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{_bucketName}.{host}/{fileKey}";
        }
    }
}