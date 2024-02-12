using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

       

        //POST: /api/Images/Upload 
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadReguestDto request)
        {
            ValidateFileUpload(request);

            if(ModelState.IsValid)
            {
                //coNVER DTO TO DOMAIN MODEL
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtention = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    
                };

                //Use repository to upload
                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);

            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadReguestDto request)
        {
            var allowedExtentions = new string[] { ".jpg", ".jpeg", ".png" };

            if(!allowedExtentions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extention");

            }
            //if the file size > 10mb
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is more than 10mb. Please add smaller file size");
            }

        }
    
    }
}
