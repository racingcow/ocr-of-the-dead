using UnityEngine;

namespace Assets.Scripts
{
    public class Word
    {
        public string FileKey { get; set; }
        public int Confidence { get; set; }
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
}