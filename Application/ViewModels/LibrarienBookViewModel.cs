using CommunityToolkit.Mvvm.ComponentModel;

namespace MyAvaloniaApp.ViewModels;

public partial class LibrarienBookViewModel(int id) : ViewModelBase
{
    [ObservableProperty]
    private string? _shortDesc = $"{App.Db.Books[id].Title} | {App.Db.Books[id].Author}";

    [ObservableProperty]
    private string? _borrowState = string.IsNullOrEmpty(App.Db.Books[id].Borrower) ? "" : $"Borrowed by {App.Db.Books[id].Borrower}";
    
    [ObservableProperty]
    private bool _borrowVisible = (App.CurrentUser == null) || App.Db.Books[id].Borrower == null;

    [ObservableProperty]
    private bool _returnVisible = (App.CurrentUser != null) && App.Db.Books[id].Borrower == App.CurrentUser.Username;
}
