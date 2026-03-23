using Avalonia.Controls;
using MyAvaloniaApp.ViewModels;
using System;
using MyAvaloniaApp.Persistence;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        // JSONPersistence persistence = new JSONPersistence("db.json");

        // Database db = new Database();
        // User user = new User()
        // {
        //     Username = "alex",
        //     Role = "librarian",
        //     PasswordHash = "123123"
        // };
        // db.Users.Add(user);
        // persistence.Save(db);

        // Database db = persistence.Load<Database>();
        // Console.WriteLine(db.Users.Count);

        DataContext = new LoginViewViewModel();
        InitializeComponent();
    }
}