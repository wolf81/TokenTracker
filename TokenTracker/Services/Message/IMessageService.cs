namespace TokenTracker.Services
{
    public enum DisplayDuration { Long, Short }

    public interface IMessageService
    {
        void Show(string message, DisplayDuration duration);
    }
}
