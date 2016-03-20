﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ocr_api.Data;

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
        /// Retrieve a file by its unique key
        /// </summary>
        /// <param name="key">The unique key of the file</param>
        /// <returns>The file</returns>
        [HttpGet]
        public FileResult Get([FromQuery]string key)
        {
            HttpContext.Response.ContentType = "application/png";

            var fileContents = _fileRepository.GetFile(key);
            var result = new FileContentResult(fileContents, "application/png")
            {
                FileDownloadName = Guid.NewGuid().ToString()
            };

            return result;
        }
    }
}
