using System.Collections.Generic;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface IWaypointList
    {
        IWaypoint Current { get; }
        IWaypoint Next { get; }
        void Add(IWaypoint item);
        void MoveNext();
    }

    public class WaypointList : LinkedList<IWaypoint>, IWaypointList
    {
        private LinkedListNode<IWaypoint> _current;

        public IWaypoint Current
        {
            get { return _current == null ? null : _current.Value; }
        }

        public IWaypoint Next
        {
            get { return _current == null ? null : _current.Next == null ? null : _current.Next.Value; }
        }

        public void Add(IWaypoint item)
        {
            AddLast(item);
        }

        public void MoveNext()
        {
            _current = _current == null ? First : _current.Next;
        }
    }
}
