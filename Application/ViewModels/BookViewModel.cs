using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Domain;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class BookViewModel : ViewModelBase
{
    public event EventHandler? BorrowStateChanged;

    private readonly Book book;

    [ObservableProperty]
    private string? _shortDesc;

    [ObservableProperty]
    private string? _borrowState;

    [ObservableProperty]
    private bool _borrowVisible;

    [ObservableProperty]
    private bool _returnVisible;

    public BookViewModel(Book book)
    {
        this.book = book;
        Update();
    }

    private void Update()
    {
        ShortDesc = $"{book.Title} | {book.Author}";
        BorrowState = string.IsNullOrEmpty(book.Borrower) ? "" : "Borrowed";
        BorrowVisible = (App.CurrentUser == null) || book.Borrower == null;
        ReturnVisible = (App.CurrentUser != null) && book.Borrower == App.CurrentUser.Username;
    }

    [RelayCommand]
    private void Return()
    {
        if (!Member.ReturnBook(App.Db, book.ISBN, App.CurrentUser?.Username!))
        {
            return;
        }

        Update();
        BorrowStateChanged?.Invoke(this, new());
    }

    [RelayCommand]
    private void Borrow()
    {
        if (!Member.BorrowBook(App.Db, book.ISBN, App.CurrentUser?.Username!))
        {
            return;
        }

        Update();
        BorrowStateChanged?.Invoke(this, new());
    }
}
