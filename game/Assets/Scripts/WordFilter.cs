using System;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Assets.Scripts
{
    // Naieve blacklist filtering
    public class WordFilter : MonoBehaviour
    {
        private static string[] _badWords;

        private void Start()
        {
            var filterFilePath = string.Format("{0}{1}", Application.dataPath, @"/Filters/BadWords.csv");
            _badWords = File.ReadAllLines(filterFilePath);
        }
        
        public static bool Contains(string word)
        {
            return _badWords.Contains(word);
        }
    }
}