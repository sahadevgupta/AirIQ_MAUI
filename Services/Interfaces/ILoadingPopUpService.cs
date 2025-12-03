namespace AirIQ.Services.Interfaces
{
    public interface ILoadingPopUpService : IDisposable
    {
        IDisposable Show();
        void Hide();
    }
}
