using MemeSource.Models;

namespace MemeSource.Interfaces
{
    public interface IBackgroundImageFetchService
    {
        FetchStatus GetCurrentStatus();
    }
}