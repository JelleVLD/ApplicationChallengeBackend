using ApplicationChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Handlers
{
    public interface IImageHandler
    {
        Task<IActionResult> UploadImage(IFormFile file);
        string DeleteImage(string fileName);
    }

    public class ImageHandler : IImageHandler
    {
        private readonly IImageWriter _imageWriter;
        public ImageHandler(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _imageWriter.UploadImage(file);
            return new ObjectResult(result);
        }
        public string DeleteImage(string fileName)
        {
            return _imageWriter.DeleteImage(fileName);
        }
    }
}
