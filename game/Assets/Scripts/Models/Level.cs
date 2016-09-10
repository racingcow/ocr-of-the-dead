using System.Collections.Generic;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface ILevel
    {
        List<Waypoint> Waypoints { get; set; }
        Waypoint CurrentWaypoint { get; set; }
    }

    public class Level : ILevel
    {
        public List<Waypoint> Waypoints { get; set; }
        public Waypoint CurrentWaypoint { get; set; }
    }
}
