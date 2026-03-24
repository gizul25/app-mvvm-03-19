using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class BookViewModel(int id) : ViewModelBase
{
    public event EventHandler? BorrowStateChanged;

    [ObservableProperty]
    private string? _shortDesc = $"{App.Db.Books[id].Title} | {App.Db.Books[id].Author}";

    [ObservableProperty]
    private string? _borrowState = string.IsNullOrEmpty(App.Db.Books[id].Borrower) ? "" : "Borrowed";
    
    [ObservableProperty]
    private bool _borrowVisible = (App.CurrentUser == null) || App.Db.Books[id].Borrower == null;

    [ObservableProperty]
    private bool _returnVisible = (App.CurrentUser != null) && App.Db.Books[id].Borrower == App.CurrentUser.Username;

    [RelayCommand]
    private void Return()
    {
        ReturnVisible = false;
        BorrowVisible = true;

        App.Db.Books[id].Borrower = null;
        BorrowStateChanged?.Invoke(this, new());
    }

    [RelayCommand]
    private void Borrow()
    {
        if (App.CurrentUser == null) return;

        BorrowVisible = false;
        ReturnVisible = true;
        
        App.Db.Books[id].Borrower = App.CurrentUser.Username;
        BorrowStateChanged?.Invoke(this, new());
    }
}
