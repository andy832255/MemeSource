using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Parameters;
using System.Text.Json;
using Serilog;
using Tweetinvi.Models;
using MemeSource.Filters;
using MemeSource.DAL.Interfaces;

namespace MemeSource.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocialMediaController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ISystemPropertyRepository _repository;

    public SocialMediaController(HttpClient httpClient, ISystemPropertyRepository repository)
    {
        _httpClient = httpClient;
        _repository = repository;
    }
    [AsyncFilter]
    [HttpGet("Test")]
    public void Test() => Response.WriteAsync("test \r\n");

    [MyFilter]
    [HttpGet("CheckUsername")]
    public async Task<IActionResult> CheckUsername()
    {
        Log.Information("CheckUsername");

        TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
        {
            Console.WriteLine(args.Url);
        };
        var TwitterToken = _repository.GetTokenAsync("Twitter");

        var credentials = new TwitterCredentials(TwitterToken.Parameter1, TwitterToken.Parameter2, TwitterToken.Parameter3, TwitterToken.Parameter4);

        var client = new TwitterClient(credentials);

        TweetinviEvents.SubscribeToClientEvents(client);

        var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();
        Console.WriteLine(authenticatedUser);
        return Ok();
    }

    [HttpGet("twitter/{username}")]
    public async Task<IActionResult> GetTwitterImages(string username)
    {
        Log.Information("GetTwitterImages");
        var images = new List<string>();

        try
        {
            var TwitterToken = _repository.GetTokenAsync("Twitter");

            var userClient = new TwitterClient(TwitterToken.Parameter1, TwitterToken.Parameter2, TwitterToken.Parameter3, TwitterToken.Parameter4);
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
        var PixivToken = _repository.GetTokenAsync("Pixiv");
        // 使用 RefreshToken取得AccessToken
        var tokenRequest = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://oauth.secure.pixiv.net/auth/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", PixivToken.Parameter1 },
                { "client_secret", PixivToken.Parameter2 },
                { "grant_type", "refresh_token" },
                { "refresh_token", PixivToken.Parameter3 }
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
