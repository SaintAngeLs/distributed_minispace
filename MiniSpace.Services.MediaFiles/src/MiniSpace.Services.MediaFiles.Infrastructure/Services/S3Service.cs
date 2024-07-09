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

        public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(fileStream, BucketName, fileName);
            return $"https://{BucketName}.s3.amazonaws.com/{fileName}";
        }

        public async Task DownloadFileAsync(string fileName, Stream destination)
        {
            var request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = fileName
            };

            using (var response = await _s3Client.GetObjectAsync(request))
            using (var responseStream = response.ResponseStream)
            {
                await responseStream.CopyToAsync(destination);
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
    }
}
