using Avalonia.Controls;
using MyAvaloniaApp.ViewModels;

namespace MyAvaloniaApp.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        DataContext = new LoginViewModel();
        InitializeComponent();
    }
}