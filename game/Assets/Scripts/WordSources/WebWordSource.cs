using System;
using System.Collections.Generic;
using System.Net;
using Racingcow.OcrOfTheDead.Models;

namespace Assets.Scripts.WordSources
{
    public class WebWordSource : IWordsSource
    {
        [Inject]
        public WebClient WebClient { get; set; }

        public event RetrieveWordsCompletedEventHandler RetrieveWordsCompleted;

        [PostConstruct]
        public void PostConstruct()
        {
            WebClient.DownloadDataCompleted += WebClientOnDownloadDataCompleted;
        }

        private void WebClientOnDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs downloadDataCompletedEventArgs)
        {
            var words = new List<IWord>();

            // todo: populate words here

            if (RetrieveWordsCompleted != null)
            {
                RetrieveWordsCompleted(this, new RetrieveWordsCompletedEventArgs {Words = words.ToArray()});
            }
        }

        public void BeginRetrieveWords(int count)
        {
            WebClient.DownloadDataAsync(new Uri("http://blahblah.com/somepath/"));
        }

        
    }
}
