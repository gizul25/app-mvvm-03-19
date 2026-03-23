using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Auth
{
    public static User? Login(Database db, string username, string password)
    {
        foreach (User user in db.Users)
        {
            if (user.Username != username)
            {
                continue;
            }
            if (user.PasswordHash != password)
            {
                return null;
            }
            return user;
        }
        
        return null;
    }
}