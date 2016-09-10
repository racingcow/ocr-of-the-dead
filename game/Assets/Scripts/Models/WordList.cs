using System;
using System.Collections;
using System.Collections.Generic;

namespace Racingcow.OcrOfTheDead.Models
{
    public interface IWordList : IList<IWord>
    {
        void ForEach(Action<IWord> action);
        void AddRange(IEnumerable<IWord> words);
        int LowCountThreshold { get; set; }
        int HighCountThreshold { get; set; }
    }

    public class WordList : IWordList
    {
        private readonly List<IWord> _words = new List<IWord>();

        public IEnumerator<IWord> GetEnumerator()
        {
            return _words.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IWord item)
        {
            _words.Add(item);
        }

        public void Clear()
        {
            _words.Clear();
        }

        public bool Contains(IWord item)
        {
            return _words.Contains(item);
        }

        public void CopyTo(IWord[] array, int arrayIndex)
        {
            _words.CopyTo(array, arrayIndex);
        }

        public bool Remove(IWord item)
        {
            return _words.Remove(item);
        }

        public int Count
        {
            get { return _words.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(IWord item)
        {
            return _words.IndexOf(item);
        }

        public void Insert(int index, IWord item)
        {
            _words.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _words.RemoveAt(index);
        }

        public IWord this[int index]
        {
            get { return _words[index]; }
            set { _words[index] = value; }
        }

        public void ForEach(Action<IWord> action)
        {
            _words.ForEach(action);
        }

        public void AddRange(IEnumerable<IWord> words)
        {
            _words.AddRange(words);
        }

        public int LowCountThreshold { get; set; }
        public int HighCountThreshold { get; set; }
    }

}