using System;
using Microsoft.AspNet.Mvc;
using ocr_api.Data;
using ocr_api.Models;

namespace ocr_api.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IFileRepository _fileRepository;

        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Retrieve a file containing a word
        /// </summary>
        /// <param name="key">Unique key of the file</param>
        /// <param name="wordBounds">Boundaries of the word</param>
        /// <returns>An image in PNG format, containing a snippet of the word</returns>
        [HttpGet]
        public FileResult Get([FromQuery] string key, [FromQuery]Rect wordBounds)
        {
            HttpContext.Response.ContentType = "application/png";

            var fileContents = _fileRepository.GetFile(key, wordBounds);
            //var fileContents = System.IO.File.ReadAllBytes(@".\Samples\servicable-parts-tag.tif");
            //var fileContents = System.IO.File.ReadAllBytes(@"E:\dev\github\ocr-of-the-dead\api\ocr-api\Samples\serviceable-parts-tag.png");
            var result = new FileContentResult(fileContents, "application/png")
            {
                FileDownloadName = Guid.NewGuid().ToString()
            };

            return result;
        }
    }
}
