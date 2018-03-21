using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Racingcow.OcrOfTheDead.Models;

namespace Assets.Scripts.WordSources
{
    public class WebWordSource : IWordsSource
    {
        //todo: move config outta here
        private const string API_BASE = "http://localhost:5000/api";
        private const int WORD_COUNT = 10;
        private const int WORD_MIN_CONF_PCT = 0;
        private const int WORD_MAX_CONF_PCT = 50;
        private const int WORD_MIN_LENGTH = 5;

        private WebClient WebClient = new WebClient();

        public event RetrieveWordsCompletedEventHandler RetrieveWordsCompleted;

        [PostConstruct]
        public void PostConstruct()
        {
            WebClient.DownloadStringCompleted += WebClientOnDownloadStringCompleted;
        }

        private void WebClientOnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            var webWords = JsonConvert.DeserializeObject<WebWord[]>(args.Result);
            var words = new ConcurrentBag<Word>();

            Parallel.ForEach(webWords, webWord => {
                var bytes = new WebClient().DownloadData($"{API_BASE}/File?key={webWord.fileKey}&X={webWord.x}&Y={webWord.y}&W={webWord.w}&H={webWord.h}");
                var word = new Word
                {
                    Id = webWord.fileKey,
                    Value = webWord.text,
                    Image = bytes,
                    ImageWidth = webWord.w,
                    ImageHeight = webWord.h
                };
                words.Add(word);
            });

            if (RetrieveWordsCompleted != null)
            {
               RetrieveWordsCompleted(this, new RetrieveWordsCompletedEventArgs {Words = words.ToArray()});
            }
        }

        public void BeginRetrieveWords(int count)
        {
            WebClient.DownloadStringAsync(new Uri($"{API_BASE}/Words?count={WORD_COUNT}&minConfPct={WORD_MIN_CONF_PCT}&maxConfPct={WORD_MAX_CONF_PCT}&minLength={WORD_MIN_LENGTH}"));
        }

    }
}
