using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

public record HackerNewsResponse
{
    public string Title { get; init; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Uri { get; set; } = string.Empty;

    [JsonPropertyName("by")]
    public string PostedBy { get; init; } = string.Empty;
    
    [JsonPropertyName("time")]
    public long UnixTime { get; init; }
    
    public int Score { get; init; }

    [JsonPropertyName("descendants")]
    public int CommentsCount { get; init; }
}