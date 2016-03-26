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

        // Use this for initialization
        void Awake () {

            // todo: get words and start background loading of images

            //var enemyCount = FindObjectsOfType<Enemy>().Length;
            var enemyCount = 50;

            using (var webClient = new WebClient())
            {
                var wordsUrl = string.Format("{0}/Words?count={1}&minConfPct={2}&maxConfPct={3}&minLength={4}",
                    API_BASE, enemyCount, CONF_MIN_PCT, CONF_MAX_PCT, WORD_MIN_CHARS);
                var wordsJson = webClient.DownloadString(wordsUrl);

                var jsonWords = (SimpleJson.JsonArray)SimpleJson.SimpleJson.DeserializeObject(wordsJson);

                var words = new List<OcrWord>();
                foreach (SimpleJson.JsonObject jsonWord in jsonWords)
                {
                    var word = new Word
                    {
                        FileKey = jsonWord["FileKey"].ToString(),
                        Text = jsonWord["Text"].ToString(),
                        X = Convert.ToInt32(jsonWord["X"]),
                        Y = Convert.ToInt32(jsonWord["Y"]),
                        W = Convert.ToInt32(jsonWord["W"]),
                        H = Convert.ToInt32(jsonWord["H"])
                    };

                    var imgUrl = string.Format("{0}/File?key={1}&X={2}&Y={3}&W={4}&H={5}",
                        API_BASE, word.FileKey, word.X, word.Y, word.W, word.H);
                    var imgData = webClient.DownloadData(imgUrl);

                    // write out snippets to disk for debugging
                    //var desiredFileName = word.FileKey + "__" + word.Text;
                    //var invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
                    //var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
                    //var fileName = System.Text.RegularExpressions.Regex.Replace(desiredFileName, invalidRegStr, "_");
                    //var filePath = @"E:\temp\scratchpad\img\" + fileName + ".png";
                    //webClient.DownloadFile(imgUrl, filePath);
                    //var imgData = File.ReadAllBytes(filePath);
                    //File.WriteAllBytes(filePath, imgData);

                    var texture = new Texture2D(word.W, word.H);
                    texture.LoadImage(imgData);

                    words.Add(new OcrWord
                    {
                        Word = word,
                        Snippet = texture
                    });
                }

                lock (_lock)
                {
                    _list = new List<OcrWord>(words);
                }
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
            var updateUrl = string.Format("{0}/Words", API_BASE);
            var word = ocrWord.Word;
            var json = SimpleJson.SimpleJson.SerializeObject(word);
            var reqData = Encoding.Default.GetBytes(json);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                webClient.UploadDataAsync(new Uri(updateUrl), "POST", reqData);
            }
        }
    }
}
