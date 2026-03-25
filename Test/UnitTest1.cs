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
using MyAvaloniaApp.Persistence;
using Xunit;
using System.Data.Common;

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
        LoginViewModel viewModel = new();
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
        LoginViewModel viewModel = new();
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

    [Fact]
    public void Persistence_Save()
    {
        File.Delete("db_test_save.json");
        Assert.False(File.Exists("db_test_save.json"));
        var persistence = new JSONPersistence("db_test_save.json");
        var db = Generator.CreateFreshDatabase();
        persistence.Save(db);
        Assert.True(File.Exists("db_test_save.json"));
    }

    // Copy "db_test_load.json" to "bin/Debug/net8.0" before running the test
    [Fact]
    public void Persistence_Load()
    {
        var persistence = new JSONPersistence("db_test_load.json");
        var db = persistence.Load<Database>();
        Assert.Equal(2, db!.Users.Count);
    }

    [Fact]
    public void Domain_AddBookExisting()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Equal(3, db.GetBooks().Count);

        Book book = new()
        {
            ISBN = "1473211549",
            Title = "Sword of Destiny",
            Author = "Andrzej Sapkowski",
            Description = "Most items will be dispatched the same or the next working day. A copy that has been read but remains in clean condition. All of the pages are intact and the cover is intact and the spine may show signs of wear. The book may have minor markings which are not specifically mentioned.",
            Borrower = null,
        };
        Assert.False(Librarian.AddBook(db, book));
        Assert.Equal(3, db.GetBooks().Count);
    }

    [Fact]
    public void Domain_AddBookNew()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Equal(3, db.GetBooks().Count);

        Book book = new()
        {
            ISBN = "1234567890",
            Title = "Example Title",
            Author = "Example Author",
            Description = "Example Description",
            Borrower = null,
        };
        Assert.True(Librarian.AddBook(db, book));
        Assert.Equal(4, db.GetBooks().Count);
    }

    [Fact]
    public void Domain_DeleteBookValid()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Equal(3, db.GetBooks().Count);

        Book book = db.GetBook("1473211549")!;
        Assert.True(Librarian.RemoveBook(db, book));
        Assert.Equal(2, db.GetBooks().Count);
    }

    [Fact]
    public void Domain_DeleteBookInvalid()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Equal(3, db.GetBooks().Count);

        Book book = db.GetBook("1234567890");
        Assert.False(Librarian.RemoveBook(db, book));
        Assert.Equal(3, db.GetBooks().Count);
    }

    [Fact]
    public void Domain_BorrowBookValid()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Empty(Member.GetBorrowedBooksByUsername(db, "steve"));

        Assert.True(Member.BorrowBook(db, "1473211549", "steve"));
        Assert.Single(Member.GetBorrowedBooksByUsername(db, "steve"));
    }

    [Fact]
    public void Domain_BorrowBookInvalid()
    {
        var db = Generator.CreateFreshDatabase();
        Assert.Empty(Member.GetBorrowedBooksByUsername(db, "steve"));

        Assert.False(Member.BorrowBook(db, "1234567890", "steve"));
        Assert.Empty(Member.GetBorrowedBooksByUsername(db, "steve"));
    }
}
