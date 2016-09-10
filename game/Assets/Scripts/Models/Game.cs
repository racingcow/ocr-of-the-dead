using System.Collections.Generic;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface IGame
    {
        List<Level> Levels { get; set; }
        Level CurrentLevel { get; set; }
    }

    public class Game : IGame
    {
        public Level CurrentLevel { get; set; }
        public List<Level> Levels { get; set; }
    }
}
