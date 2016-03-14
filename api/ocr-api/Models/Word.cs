namespace ocr_api.Models
{
  public class Word
  {
    public int Id { get; set; }
    public int Confidence { get; set; }
    public string Text { get; set; }
  }
}