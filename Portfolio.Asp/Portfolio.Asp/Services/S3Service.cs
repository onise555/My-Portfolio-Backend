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
            // Railway-ს Variables ჩანართიდან იღებს მნიშვნელობებს
            var accessKey = config["AWS_ACCESS_KEY_ID"] ?? config["S3Config:AccessKey"];
            var secretKey = config["AWS_SECRET_ACCESS_KEY"] ?? config["S3Config:SecretKey"];

            // აქ მნიშვნელოვანია: გამოიყენე ზუსტად ის სახელი, რომელიც მუშა ლინკშია (images-55)
            _bucketName = config["AWS_S3_BUCKET_NAME"] ?? config["S3Config:BucketName"] ?? "images-55";

            // ენდპოინტი - შენი მუშა ლინკის მიხედვით: https://t3.storage.dev
            _serviceUrl = config["AWS_ENDPOINT_URL"] ?? config["S3Config:ServiceUrl"] ?? "https://t3.storage.dev";

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                ForcePathStyle = false, // აუცილებელია, რომ ლინკი bucket.endpoint ფორმატში იყოს
                UseHttp = false
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);
        }

        public async Task<string?> UploadFileAsync(IFormFile? file, string folder = "users/images")
        {
            if (file == null || file.Length == 0) return null;

            // უნიკალური ID ფაილისთვის
            var fileKey = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead, // ააქტიურებს საჯარო წვდომას
                DisablePayloadSigning = true
            };

            // ატვირთვა S3-ზე
            await _s3Client.PutObjectAsync(putRequest);

            // ვაწყობთ ზუსტად იმ მუშა ლინკს, რომელიც მომწერე
            // შედეგი: https://images-55.t3.storage.dev/users/images/filename.mp4
            var host = _serviceUrl.Replace("https://", "").TrimEnd('/');
            return $"https://{_bucketName}.{host}/{fileKey}";
        }
    }
}