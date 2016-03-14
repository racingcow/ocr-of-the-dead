using Microsoft.AspNet.Mvc;

namespace ocr_api.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        [HttpGet]
        public FileResult Get()
        {
            HttpContext.Response.ContentType = "application/png";
            var result = new FileContentResult(System.IO.File.ReadAllBytes(@"c:\test.png"), "application/png")
            {
                FileDownloadName = "TheFile.png"
            };
            return result;
        }
    }
}
