using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Domain;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _statusMessage = "";

    [RelayCommand]
    private void Login()
    {
        User? user = Auth.Login(App.Db!, Username, Password);
        if (user == null)
        {
            StatusMessage = "Invalid user";
            return;
        }

        App.CurrentUser = user;
        StatusMessage = "";
        App.ChangePageTo((user.Role == "member") ? new CatalogViewModel() : new LibrarianCatalogViewModel());
    }
}