namespace Racingcow.OcrOfTheDead.Signals
{
    public class AimInfo
    {
        public AimInfo(AimDirection direction)
        {
            Direction = direction;
        }

        public enum AimDirection
        {
            Next,
            Previous
        }

        public AimDirection Direction { get; set; }
    }
}