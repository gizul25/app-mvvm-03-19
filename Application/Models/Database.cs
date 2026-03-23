using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class Database
{
    [JsonPropertyName("users")]
    public List<User> Users { get; set; } = new();
}