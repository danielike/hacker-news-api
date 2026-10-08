using System.Text.Json.Serialization;

namespace HackerNewsApi.Models;

public record News
{
    public string Title { get; init; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Uri { get; set; } = string.Empty;
    
    [JsonPropertyName("by")]
    public string PostedBy { get; init; } = string.Empty;
    
    public string Time { get; init; } = string.Empty;
    
    public int Score { get; init; }
    
    public int CommentsCount { get; init; }
}