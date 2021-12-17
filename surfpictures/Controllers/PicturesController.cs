using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace surfpictures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PicturesController : ControllerBase
    {
        private readonly IAmazonS3 amazonS3;
        public PicturesController(IAmazonS3 amazonS3)
        {
            this.amazonS3 = amazonS3;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files)
        {
            for (int i=0; i < files.Count; i++)
            {
                var putRequest = new PutObjectRequest()
                {
                    BucketName = "surfuruguay",
                    Key = files[i].FileName,
                    InputStream = files[i].OpenReadStream(),
                    ContentType = files[i].ContentType
                };
                var result = await this.amazonS3.PutObjectAsync(putRequest);
                if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    return BadRequest(result);
                }
            }
            return Ok("Upload successfull."); 
        }
    }
}
