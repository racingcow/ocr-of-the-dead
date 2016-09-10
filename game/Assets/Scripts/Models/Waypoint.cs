namespace Racingcow.OcrOfTheDead.Models
{
    public interface IWaypoint
    {
        int Sequence { get; set; }
        string Name { get; set; }
        IEnemyList Enemies { get; set; }
    }

    public class Waypoint : IWaypoint
    {
        public int Sequence { get; set; }
        public string Name { get; set; }

        [Inject]
        public IEnemyList Enemies { get; set; }
    }
}
