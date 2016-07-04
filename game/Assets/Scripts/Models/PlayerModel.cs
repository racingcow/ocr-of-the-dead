namespace Racingcow.OcrOfTheDead.Models
{
    public interface IPlayerModel
    {
        int Health { get; set; }
    }

    public class PlayerModel : IPlayerModel
    {
        public int Health { get; set; }
    }
}
