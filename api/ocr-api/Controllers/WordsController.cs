using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ocr_api.Models;

namespace ocr_api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WordsController : Controller
    {
        /// <summary>
        /// Get OCR words that need correction
        /// </summary>
        /// <param name="count">Desired number of words</param>
        /// <param name="minConfPct">Minimum confidence threshold percentage</param>
        /// <param name="maxConfPct">Maximum confidence threshold percentage</param>
        /// <returns>List of words meeting the specified criteria</returns>
        [HttpGet]
        public IEnumerable<Word> Get(int count = 50, int minConfPct = 50, int maxConfPct = 85)
        {
            var words = new List<Word>();
            for (var i = 0; i < 10; i++)
            {
                words.Add(new Word { Id = i, Text = $"Word{i}" });
            }
            return words;
        }
    }
}