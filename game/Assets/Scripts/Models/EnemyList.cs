using System;
using System.Collections;
using System.Collections.Generic;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface IEnemyList : IList<IEnemy>
    {
        void ForEach(Action<IEnemy> action);
    }

    public class EnemyList : IEnemyList
    {
        private readonly List<IEnemy> _enemies = new List<IEnemy>();

        public IEnumerator<IEnemy> GetEnumerator()
        {
            return _enemies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IEnemy item)
        {
            _enemies.Add(item);
        }

        public void Clear()
        {
            _enemies.Clear();
        }

        public bool Contains(IEnemy item)
        {
            return _enemies.Contains(item);
        }

        public void CopyTo(IEnemy[] array, int arrayIndex)
        {
            _enemies.CopyTo(array, arrayIndex);
        }

        public bool Remove(IEnemy item)
        {
            return _enemies.Remove(item);
        }

        public int Count
        {
            get { return _enemies.Count; } 
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(IEnemy item)
        {
            return _enemies.IndexOf(item);
        }

        public void Insert(int index, IEnemy item)
        {
            _enemies.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _enemies.RemoveAt(index);
        }

        public IEnemy this[int index]
        {
            get { return _enemies[index]; }
            set { _enemies[index] = value; }
        }

        public void ForEach(Action<IEnemy> action)
        {
            _enemies.ForEach(action);
        }
    }

}