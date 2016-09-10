namespace Racingcow.OcrOfTheDead.Models
{
    public interface IPlayer
    {
        int Health { get; set; }
        int DamageAmount { get; set; }
        string Name { get; set; }
    }

    public class Player : IPlayer
    {
        public int Health { get; set; }
        public int DamageAmount { get; set; }
        public string Name { get; set; }
    }
}
