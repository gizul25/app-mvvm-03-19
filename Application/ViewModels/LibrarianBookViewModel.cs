using CommunityToolkit.Mvvm.ComponentModel;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class LibrarianBookViewModel(Book book) : ViewModelBase
{
    [ObservableProperty]
    private string? _shortDesc = $"{book.Title} | {book.Author}";

    [ObservableProperty]
    private string? _borrowState = string.IsNullOrEmpty(book.Borrower) ? "" : $"Borrowed by {book.Borrower}";

    [ObservableProperty]
    private bool _borrowVisible = (App.CurrentUser == null) || book.Borrower == null;

    [ObservableProperty]
    private bool _returnVisible = (App.CurrentUser != null) && book.Borrower == App.CurrentUser.Username;
}
