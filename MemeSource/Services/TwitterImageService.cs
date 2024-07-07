using Microsoft.Extensions.Options;
using MemeSource.Models;
using MemeSource.Interfaces;

namespace MemeSource.Services
{
    public class TwitterImageService : ITwitterImageService
    {
        private readonly TwitterConfig _twitterConfig;

        public TwitterImageService(IOptions<TwitterConfig> twitterConfig)
        {
            _twitterConfig = twitterConfig.Value;
        }

        public async Task FetchLatestTwitterImages()
        {
            #warning todo
        }
    }
}