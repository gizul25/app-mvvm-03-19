using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class Database
{
    [JsonPropertyName("users")]
    public List<User> Users { get; set; } = new();

    [JsonPropertyName("books")]
    public List<Book> Books { get; set; } = new();
}