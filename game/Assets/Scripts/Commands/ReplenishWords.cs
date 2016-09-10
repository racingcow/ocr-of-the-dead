using Assets.Scripts.WordSources;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;
using strange.extensions.command.impl;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class ReplenishWords : Command
    {
        [Inject]
        public IWordList WordsList { get; set; }

        [Inject]
        public IWordsSource WordSource { get; set; }

        [Inject]
        public WordsHighCountReached WordsHighCountReached { get; set; }

        public override void Execute()
        {
            Retain();

            var neededWordCt = WordsList.HighCountThreshold - WordsList.Count;
            WordSource.RetrieveWordsCompleted += WordSourceOnRetrieveWordsCompleted;
            WordSource.BeginRetrieveWords(neededWordCt);
        }

        private void WordSourceOnRetrieveWordsCompleted(object sender, RetrieveWordsCompletedEventArgs args)
        {
            WordsList.AddRange(args.Words);
            WordsHighCountReached.Dispatch();
            WordSource.RetrieveWordsCompleted -= WordSourceOnRetrieveWordsCompleted;
            Release();
        }
    }
}