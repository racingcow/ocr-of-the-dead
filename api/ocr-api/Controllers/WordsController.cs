using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using ocr_api.Data;
using ocr_api.Models;

namespace ocr_api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WordsController : Controller
    {
        private readonly IWordRepository _repo;

        public WordsController(IWordRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Get OCR words that need correction
        /// </summary>
        /// <param name="count">Desired number of words</param>
        /// <param name="minConfPct">Minimum confidence threshold percentage</param>
        /// <param name="maxConfPct">Maximum confidence threshold percentage</param>
        /// <param name="minLength">Words returned must have at least this many characters found in the OCR'd text</param>
        /// <returns>List of words meeting the specified criteria</returns>
        [HttpGet]
        public IEnumerable<Word> Get([FromQuery]int count = 50, [FromQuery]int minConfPct = 50, [FromQuery]int maxConfPct = 85, [FromQuery]int minLength = 5)
        {
            return _repo.Get(count, minConfPct, maxConfPct, minLength);
        }

        /// <summary>
        /// AddCorrection a word with corrected text
        /// </summary>
        /// <param name="word">Updated word</param>
        [HttpPost]
        public void Post([FromBody]Word word)
        {
            _repo.AddCorrection(word);
        }
    }
}