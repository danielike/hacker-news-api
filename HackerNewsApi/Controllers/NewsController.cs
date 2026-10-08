using System.Globalization;
using Asp.Versioning;
using HackerNewsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class NewsController : ControllerBase
{
    private readonly HttpClient _hackerNewsClient;
    private readonly IMemoryCache _cache;

    public NewsController(IHttpClientFactory clientFactory, IMemoryCache cache)
    {
        _hackerNewsClient = clientFactory.CreateClient(nameof(HttpClients.HttpClients.HackerNews));
        _cache = cache;
    }
    
    [MapToApiVersion("1.0")]
    [HttpGet("")]
    public async Task<IEnumerable<News>> Get([FromQuery] int amount)
    {
        var storyIds = await _hackerNewsClient.GetFromJsonAsync<int[]>("/v0/beststories.json") ?? [];
        
        var tasks = storyIds.Select(async storyId =>
        {
            if (!_cache.TryGetValue($"news:{storyId}", out var news))
            {
                var response = await _hackerNewsClient.GetFromJsonAsync<HackerNewsResponse>($"/v0/item/{storyId}.json");
                var fetchedNews = new News
                {
                    Uri = response!.Uri,
                    PostedBy = response.PostedBy,
                    Time = response.UnixTime is 0 ? "" : DateTimeOffset
                        .FromUnixTimeSeconds(response.UnixTime)
                        .ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture),
                    CommentsCount = response.CommentsCount,
                    Score = response.Score,
                    Title = response.Title
                };
                
                _cache.Set($"news:{storyId}", fetchedNews, absoluteExpirationRelativeToNow: TimeSpan.FromMinutes(5));
                
                return fetchedNews;
            }

            return news as News;
        });

        var result = await Task.WhenAll(tasks);
        
        return result
                .OrderByDescending(x => x!.Score)
                .Take(amount is 0 ? int.MaxValue : amount)!;
    }
}