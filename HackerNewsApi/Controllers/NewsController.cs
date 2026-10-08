using System.Globalization;
using Asp.Versioning;
using HackerNewsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class NewsController : ControllerBase
{
    private readonly HttpClient _hackerNewsClient;

    public NewsController(IHttpClientFactory clientFactory)
    {
        _hackerNewsClient = clientFactory.CreateClient(nameof(HttpClients.HttpClients.HackerNews));
    }
    
    [MapToApiVersion("1.0")]
    [HttpGet("")]
    public async Task<IEnumerable<News>> Get()
    {
        var storyIds = await _hackerNewsClient.GetFromJsonAsync<int[]>("/v0/beststories.json") ?? [];

        var news = new List<News>();

        var tasks = storyIds.Select(async storyId =>
        {
            var response = await _hackerNewsClient.GetFromJsonAsync<HackerNewsResponse>($"/v0/item/{storyId}.json");

            return new News
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
        });
        
        
        return await Task.WhenAll(tasks);
    }
}