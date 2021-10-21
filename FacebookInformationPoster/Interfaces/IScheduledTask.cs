namespace FacebookInformationPoster
{
    public interface IScheduledTask
    {
        string Name { get; }
        void Execute();
    }
}
