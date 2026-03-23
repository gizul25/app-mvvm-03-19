using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class Book
{
    [JsonPropertyName("isbn")]
    public required string ISBN { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("author")]
    public required string Author { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("borrower")]
    public required string? Borrower { get; set; } = null;
}