using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers


{
    [Route ("api/")]
    [ApiController]
    [AllowAnonymous]
    public class UploadMP3Controller: ControllerBase
    {
        public readonly BlobServiceClient _blobServiceClient;


        public UploadMP3Controller(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        [HttpPost("upload_convert_mp3")]
        public Task<IActionResult> Upload(IFormFile file)
        {
            //if (file == null || file.Length == 0) return BadRequest("Please upload a file.");

            //var containerClient = _blobServiceClient.GetBlobContainerClient("musikstorage");
            ////create container if not exist
            //await containerClient.CreateIfNotExistsAsync();

            //var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.Name));

            //await using (var stream = file.OpenReadStream()) { await blobClient.UploadAsync(stream, true); }

            //var shortUrl = blobClient.Uri.ToString();
            //return Ok(shortUrl);

            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                error = "Service is no longer available due to Azure expiration."
            }
            ));
        }
    }
}
