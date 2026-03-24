using CommunityToolkit.Mvvm.ComponentModel;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private User? _currentUser;
    
    [ObservableProperty]
    private ObservableObject _currentPage = new LoginViewModel();

    public MainWindowViewModel()
    {
        App.ChangePage += ChangePage;
    }

    private void ChangePage(object? sender, ChangePageArgs args)
    {
        CurrentPage = args.Page;
    }
}