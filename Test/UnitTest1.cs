using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using MyAvaloniaApp.ViewModels;
using MyAvaloniaApp.Views;
using Xunit;

namespace MyAvaloniaApp.Headless.XUnit;

public class UnitTest1
{
    [AvaloniaFact]
    public void Window_WelcomeText()
    {
        // Create a window and set the view model as its data context:
        var window = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        // Show the window, as it's required to get layout processed:
        window.Show();

        MainWindowViewModel ctx = (MainWindowViewModel)window.DataContext;
        Assert.Equal("Welcome to Avalonia!", ctx.Greeting);
    }

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
}
