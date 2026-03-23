using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class User
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("role")]
    public required string Role { get; set; }

    [JsonPropertyName("password_hash")]
    public required string PasswordHash { get; set; }
}