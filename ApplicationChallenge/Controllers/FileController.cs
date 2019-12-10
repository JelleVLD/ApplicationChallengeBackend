using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationChallenge.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationChallenge.Controllers
{
    [Route("api/image")]
    public class ImagesController : Controller
    {
        private readonly IImageHandler _imageHandler;

        public ImagesController(IImageHandler imageHandler)
        {
            _imageHandler = imageHandler;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            return await _imageHandler.UploadImage(file);
        }
        [HttpDelete("{fileName}")]
        public string DeleteImage(string fileName)
        {
            _imageHandler.DeleteImage(fileName);
            return "ok";
        }
    }
}
