namespace AirIQ.Services.Interfaces
{
    public interface ILoadingPopUpService : IDisposable
    {
        IDisposable Show();
        void Hide();

        /// <summary>
        /// Same as <see cref="Hide"/>, but returns the task that completes once the popup has
        /// actually been removed. Use this before doing something highly visible right after
        /// hiding (e.g. navigating to a new page) so the popup can't still be mid-dismiss - and
        /// therefore stuck rendered on top of the new page - when that happens.
        /// </summary>
        Task HideAsync();
    }
}
