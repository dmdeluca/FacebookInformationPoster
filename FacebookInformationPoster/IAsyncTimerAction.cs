namespace FacebookInformationPoster
{
    public interface IAsyncTimerAction
    {
        void Action();
        void Dispose();
        void OnConfiguring(AsyncTimerActionOptions options);
        void Start();
    }
}