using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MiniSpace.Services.MediaFiles.Application.Services;
using System.IO;
using System.Threading.Tasks;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "minispace-data-files";

        public S3Service(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(string folderName, string fileName, Stream fileStream)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            var key = $"{folderName}/{fileName}";

            // If the file size is larger than a certain threshold, use multipart upload
            if (fileStream.Length > 5 * 1024 * 1024) // 5 MB
            {
                var request = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    Key = key,
                    BucketName = BucketName,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 5 * 1024 * 1024, // 5 MB
                    ContentType = "image/webp"
                };
                await fileTransferUtility.UploadAsync(request);
            }
            else
            {
                await fileTransferUtility.UploadAsync(fileStream, BucketName, key);
            }
            
            return $"https://{BucketName}.s3.amazonaws.com/{key}";
        }

        public async Task DownloadFileAsync(string fileUrl, Stream destination)
        {
            var uri = new Uri(fileUrl);
            var request = new GetObjectRequest
            {
                BucketName = uri.Host.Split('.')[0],
                Key = uri.AbsolutePath.Substring(1)
            };

            using (var response = await _s3Client.GetObjectAsync(request))
            using (var responseStream = response.ResponseStream)
            {
                await responseStream.CopyToAsync(destination);
            }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = uri.Host.Split('.')[0],
                Key = uri.AbsolutePath.Substring(1)
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
    }
}
