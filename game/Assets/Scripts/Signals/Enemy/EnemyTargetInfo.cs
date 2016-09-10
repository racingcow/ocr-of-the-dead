using Racingcow.OcrOfTheDead.Models;

namespace Racingcow.OcrOfTheDead.Signals
{
    public class EnemyTargetInfo
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string Word { get; set; }
        public bool Targeted { get; set; }
    }
}