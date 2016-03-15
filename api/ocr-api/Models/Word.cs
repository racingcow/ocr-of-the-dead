namespace ocr_api.Models
{
  public class Word
  {
    public string Key { get; set; }
    public int Confidence { get; set; }
    public string Text { get; set; }
  }
}