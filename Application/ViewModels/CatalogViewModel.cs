using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class CatalogViewModel : ViewModelBase
{
    private bool onlyBorrowed = false;
    private int lastDetailedBook = -1;

    [ObservableProperty]
    private bool _showBookDetails;

    [ObservableProperty]
    private string? _bookDisc;

    [ObservableProperty]
    private IEnumerable _bookList;

    [ObservableProperty]
    private int _spaceSize;

    [RelayCommand]
    private void Logout()
    {
        App.CurrentUser = null;
        App.ChangePageTo(new LoginViewModel());
    }

    [RelayCommand]
    private void Borrowed()
    {
        onlyBorrowed = !onlyBorrowed;
        BookList = Books(App.Db.Books);
    }

    [RelayCommand]
    public void ShowDetails(int? id)
    {
        if (id == null)
        {
            ShowBookDetails = false;
            SpaceSize = 2;
            return;
        }

        if (lastDetailedBook == id)
        {
            ShowBookDetails = false;
            SpaceSize = 2;
            lastDetailedBook = -1;
            return;
        }

        SpaceSize = 1;
        lastDetailedBook = (int)id;
        ShowBookDetails = true;
        BookDisc =
            $"{App.Db.Books[(int)id].Title}\n" +
            $"From: {App.Db.Books[(int)id].Author}\n" +
            $"ISBN: {App.Db.Books[(int)id].ISBN}\n\n" +
            $"Description: {App.Db.Books[(int)id].Description}\n";
    }

    public CatalogViewModel()
    {
        BookList = Books(App.Db.Books);
        SpaceSize = 2;
    }

    private ObservableCollection<ViewModelBase> Books(List<Book> library)
    {
        ObservableCollection<ViewModelBase> books = [];
        if (App.CurrentUser == null)
        {
            return books;
        }

        for (int id = 0; id < library.Count; id++)
        {
            Book book = library[id];
            if (!onlyBorrowed || App.CurrentUser.Username == book.Borrower)
            {
                BookViewModel bookModel = new(id);
                bookModel.BorrowStateChanged += Relist;
                books.Add(bookModel);
            }
        }
        return books;
    }

    private void Relist(object? sender, EventArgs e)
    {
        BookList = Books(App.Db.Books);
    }
}
