using Racingcow.OcrOfTheDead.Enums;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface IEnemy
    {
        string Name { get; set; }
        IWaypoint Waypoint { get; set; }
        EnemyStates State { get; set; }
        bool Targeted { get; set; }
        int Health { get; set; }
        IWord Word { get; set; }
        int DamageAmount { get; set; }
    }

    public class Enemy : IEnemy
    {
        public bool Targeted { get; set; }
        public int Health { get; set; }
        public IWord Word { get; set; }
        public string Name { get; set; }
        public IWaypoint Waypoint { get; set; }
        public EnemyStates State { get; set; }
        public int DamageAmount { get; set; }
    }
}
