using ocr_api.Models;

namespace ocr_api.Data
{
    public interface IWordRepository
    {
        Word[] Get(int count, int minConfPct, int maxConfPct, int minLength);
    }
}