using MemeSource.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MemeSource.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageFetchController : ControllerBase
    {
        private readonly ITwitterImageService _twitterImageService;
        private readonly IBackgroundImageFetchService _backgroundService;

        public ImageFetchController(ITwitterImageService twitterImageService, IBackgroundImageFetchService backgroundService)
        {
            _twitterImageService = twitterImageService;
            _backgroundService = backgroundService;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> FetchImages()
        {
            await _twitterImageService.FetchLatestTwitterImages();
            return Ok("Image fetch process started.");
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var status = _backgroundService.GetCurrentStatus();
            return Ok(status);
        }
    }
}