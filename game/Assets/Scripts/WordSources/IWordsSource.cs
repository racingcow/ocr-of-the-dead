namespace Assets.Scripts.WordSources
{
    public interface IWordsSource
    {
        void BeginRetrieveWords(int count);
        event RetrieveWordsCompletedEventHandler RetrieveWordsCompleted;
    }
}
