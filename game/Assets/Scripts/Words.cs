using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Words : MonoBehaviour
    {
        private static List<OcrWord> _list;
        private static readonly object _lock = new object();

        private const string API_BASE = "http://localhost:5001/api";
        private const int CONF_MIN_PCT = 0;
        private const int CONF_MAX_PCT = 75;
        private const int WORD_MIN_CHARS = 3;

        private string ParseId(string fileName)
        {
            var startIdx = fileName.IndexOf('(') + 1;
            var endIdx = fileName.IndexOf(')') - 1;
            var length = endIdx - startIdx + 1;
            var id = fileName.Substring(startIdx, length);
            return id;
        }

        // Use this for initialization
        void Awake () {

            var words = new List<OcrWord>();
            foreach (var file in Directory.GetFiles(@"E:\temp\scratchpad\img", "*.png"))
            {
                var id = ParseId(file);
                
                var imgData = File.ReadAllBytes(file);

                var texture = new Texture2D(400, 50);
                texture.LoadImage(imgData);

                words.Add(new OcrWord
                {
                    Word = new Word
                    {
                        FileKey = id
                    },
                    Snippet = texture
                });
                
               _list = new List<OcrWord>(words);
            }
        }
	
        // Update is called once per frame
        void Update ()
        {
	
        }

        /// <summary>
        /// Remove a ocrWord randomly from the words collection
        /// </summary>
        /// <returns>The ocrWord that was removed</returns>
        public static OcrWord RemoveRandom()
        {
            lock (_lock)
            {
                var idx = UnityEngine.Random.Range(0, _list.Count);
                var word = _list[idx];
                _list.RemoveAt(idx);
                return word;
            }
        }

        public static void AddCorrection(OcrWord ocrWord)
        {
        }
    }
}
