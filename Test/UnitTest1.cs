using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using System.Diagnostics;
using MyAvaloniaApp;
using MyAvaloniaApp.ViewModels;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Views;
using MyAvaloniaApp.Domain;
using Xunit;

namespace TestableApp.Headless.XUnit;

public class UnitTest1
{
    [AvaloniaFact]
    public void Avalonia_TestWorking()
    {
        // Setup controls:
        var textBox = new TextBox();
        var window = new Window { Content = textBox };

        // Open window:
        window.Show();

        // Focus text box:
        textBox.Focus();

        // Simulate text input:
        window.KeyTextInput("Hello World");

        // Assert:
        Assert.Equal("Hello World", textBox.Text);
    }

    [AvaloniaFact]
    public void LoginView_LoginInvalid()
    {
        LoginViewViewModel viewModel = new();
        LoginView page = new LoginView
        {
            DataContext = viewModel
        };

        Window window = new Window
        {
            Content = page
        };
        window.Show();

        Database db = new();
        string passwordHash = Auth.GenerateHashAndSalt("123123");
        User user = new()
        {
            Username = "alex",
            Role = "librarian",
            PasswordHash = passwordHash,
        };
        db.Users.Add(user);
        App.Db = db;

        TextBox usernameInput = page.UsernameInput;
        usernameInput.Text = "asdasd";
        TextBox passwordInput = page.PasswordInput;
        passwordInput.Text = "asdasd";

        Button loginBtn = page.LoginBtn;
        loginBtn.Focus();
        window.KeyReleaseQwerty(PhysicalKey.Space, RawInputModifiers.None);

        TextBlock statusText = page.StatusText;
        Assert.Equal("Invalid user", statusText.Text);
    }

    [AvaloniaFact]
    public void LoginView_LoginValid()
    {
        LoginViewViewModel viewModel = new();
        LoginView page = new LoginView
        {
            DataContext = viewModel
        };

        Window window = new Window
        {
            Content = page
        };
        window.Show();

        Database db = new();
        string passwordHash = Auth.GenerateHashAndSalt("123123");
        User user = new()
        {
            Username = "alex",
            Role = "librarian",
            PasswordHash = passwordHash,
        };
        db.Users.Add(user);
        
        App.Db = db;

        TextBox usernameInput = page.UsernameInput;
        usernameInput.Text = "alex";
        TextBox passwordInput = page.PasswordInput;
        passwordInput.Text = "123123";

        Button loginBtn = page.LoginBtn;
        loginBtn.Focus();
        window.KeyReleaseQwerty(PhysicalKey.Space, RawInputModifiers.None);

        TextBlock statusText = page.StatusText;
        Assert.Equal("", statusText.Text);
    }
}
