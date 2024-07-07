using MemeSource.Interfaces;
using MemeSource.Models;

namespace MemeSource.Services
{
    public class BackgroundImageFetchService : BackgroundService, IBackgroundImageFetchService
    {
        private readonly ITwitterImageService _twitterImageService;
        private FetchStatus _currentStatus;

        public BackgroundImageFetchService(ITwitterImageService twitterImageService)
        {
            _twitterImageService = twitterImageService;
            _currentStatus = new FetchStatus { LastFetchTime = DateTime.MinValue, IsRunning = false };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _currentStatus.IsRunning = true;
                await _twitterImageService.FetchLatestTwitterImages();
                _currentStatus.LastFetchTime = DateTime.Now;
                _currentStatus.IsRunning = false;

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        public FetchStatus GetCurrentStatus() => _currentStatus;
    }
}