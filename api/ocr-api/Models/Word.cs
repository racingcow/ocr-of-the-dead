namespace ocr_api.Models
{
    public class Word
    {
        /// <summary>
        /// Unique identifier of the file containing this word
        /// </summary>
        public string FileKey { get; set; }

        /// <summary>
        /// Percent confidence (between 0 and 100) that the <paramref name="Text"/> of this Word was correctly read by the OCR process
        /// </summary>
        public int Confidence { get; set; }

        /// <summary>
        /// The text characters read by the OCR process for this word
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The left coordinate of the bounding rectangle of the word, in pixels
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The top coordinate of the bounding rectangle of the word, in pixels
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The width of the bounding rectangle of the word, in pixels
        /// </summary>
        public int W { get; set; }

        /// <summary>
        /// The height of the bounding rectangle of the word, in pixels
        /// </summary>
        public int H { get; set; }
    }
}