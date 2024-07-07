using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Tweetinvi;
using Tweetinvi.Parameters;
using System.Text.Json;
using MemeSource.Models;
using Serilog;
using Tweetinvi.Models;

namespace SocialMediaCrawlerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialMediaController : ControllerBase
    {
        private readonly TwitterConfig _twitterConfig;
        private readonly PixivConfig _pixivConfig;
        private readonly HttpClient _httpClient;

        public SocialMediaController(IOptions<TwitterConfig> twitterConfig, IOptions<PixivConfig> pixivConfig, HttpClient httpClient)
        {
            _twitterConfig = twitterConfig.Value;
            _pixivConfig = pixivConfig.Value;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> CheckUsername()
        {
            Log.Information("CheckUsername");

            TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
            {
                System.Console.WriteLine(args.Url);
            };

            var credentials = new TwitterCredentials(_twitterConfig.APIKey, _twitterConfig.APIKeySecret, _twitterConfig.AccessToken, _twitterConfig.AccessTokenSecret);
#warning todo
            
            var client = new TwitterClient(credentials);

            TweetinviEvents.SubscribeToClientEvents(client);

            var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();
            System.Console.WriteLine(authenticatedUser);
            return Ok();
        }

        [HttpGet("twitter/{username}")]
        public async Task<IActionResult> GetTwitterImages(string username)
        {
            Log.Information("GetTwitterImages");
            var images = new List<string>();

            try
            {
                var userClient = new TwitterClient(_twitterConfig.APIKey, _twitterConfig.APIKeySecret, _twitterConfig.AccessToken, _twitterConfig.AccessTokenSecret);
                var user = await userClient.Users.GetUserAsync(username);
                var timelineParameters = new GetUserTimelineParameters(user)
                {
                    IncludeRetweets = false,
                    ExcludeReplies = true,
                    PageSize = 50
                };
                var timeline = await userClient.Timelines.GetUserTimelineAsync(timelineParameters);

                foreach (var tweet in timeline)
                {
                    if (tweet.Entities.Medias != null)
                    {
                        foreach (var media in tweet.Entities.Medias)
                        {
                            if (media.MediaType == "photo")
                            {
                                images.Add(media.MediaURLHttps);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return Ok(images);
        }

        [HttpGet("pixiv/{userid}")]
        public async Task<IActionResult> GetPixivImages(string userid)
        {
            // 使用 RefreshToken取得AccessToken
            var tokenRequest = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://oauth.secure.pixiv.net/auth/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", _pixivConfig.ClientId },
                    { "client_secret", _pixivConfig.ClientSecret },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", _pixivConfig.RefreshToken }
                })
            };

            var tokenResponse = await _httpClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            var tokenJson = JsonDocument.Parse(tokenContent);
            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            // 插圖
            var illustRequest = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"https://app-api.pixiv.net/v1/user/illusts?user_id={userid}&filter=for_ios");
            illustRequest.Headers.Add("Authorization", $"Bearer {accessToken}");

            var illustResponse = await _httpClient.SendAsync(illustRequest);
            illustResponse.EnsureSuccessStatusCode();
            var illustContent = await illustResponse.Content.ReadAsStringAsync();
            var illustJson = JsonDocument.Parse(illustContent);

            var images = new List<string>();
            foreach (var illust in illustJson.RootElement.GetProperty("illusts").EnumerateArray())
            {
                var imageUrl = illust.GetProperty("image_urls").GetProperty("large").GetString();
                images.Add(imageUrl);
            }

            return Ok(images);
        }
    }
}
