namespace FacebookInformationPoster
{
    public interface IScheduler
    {
        void Dispose();
        void StartPolling();
    }
}