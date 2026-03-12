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
            // Fallback: Railway-ს ავტომატური ცვლადები -> appsettings
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "coordinated-pocket-nxuvrv";
            _serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storageapi.dev";

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("S3 Credentials are not configured! Check Railway Variables.");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(credentials, new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                ForcePathStyle = true, // აუცილებელია T3/Cloudflare/Minio-სთვის
                AuthenticationRegion = "auto"
            });
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folder)
        {
            if (file == null || file.Length == 0) return null;

            // სწორი გაფართოების შენარჩუნება (.jpg, .mp4 და ა.შ.)
            var extension = Path.GetExtension(file.FileName);
            var fileKey = $"{folder}/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true,
                CannedACL = S3CannedACL.PublicRead // აუცილებელია, რომ ლინკი გაიხსნას!
            };

            try
            {
                await _s3Client.PutObjectAsync(putRequest);

                // T3-ზე საჯარო ლინკის ფორმატი: https://t3.storageapi.dev/bucket/folder/file
                var baseUrl = _serviceUrl.TrimEnd('/');
                return $"{baseUrl}/{_bucketName}/{fileKey}";
            }
            catch (Exception ex)
            {
                throw new Exception($"S3 Upload failed: {ex.Message}");
            }
        }
    }
}