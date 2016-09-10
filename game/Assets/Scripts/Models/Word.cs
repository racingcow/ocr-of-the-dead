namespace Racingcow.OcrOfTheDead.Models
{
    public interface IWord
    {
        string Id { get; set; }
        string Value { get; set; }
        byte[] Image { get; set; }
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
    }

    public class Word : IWord
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public byte[] Image { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
    }
}
