using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Domain;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.ViewModels;

public partial class LibrarianCatalogViewModel : ViewModelBase
{
    private bool onlyBorrowed = false;
    private int lastDetailedBook = -1;

    [ObservableProperty]
    private bool _showBookDetails;

    [ObservableProperty]
    private bool _canRemove = true;

    [ObservableProperty]
    private string _titleBind = "";

    [ObservableProperty]
    private string _authorBind = "";

    [ObservableProperty]
    private string _isbnBind = "";

    [ObservableProperty]
    private string _bookDisc = "";

    [ObservableProperty]
    private IEnumerable _bookList;

    [ObservableProperty]
    private int _spaceSize;

    public LibrarianCatalogViewModel()
    {
        SpaceSize = 2;
        Update();
    }

    private void Update()
    {
        List<Book> books;
        if (!onlyBorrowed)
        {
            books = App.Db.GetBooks();
        }
        else
        {
            books = Librarian.GetBorrowedBooks(App.Db!);
        }
        BookList = MapBooks(books);
    }

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
        Update();
    }

    [RelayCommand]
    private void Remove()
    {
        var book = App.Db.GetBookByIndex(lastDetailedBook);
        if (book == null)
        {
            return;
        }

        Librarian.RemoveBook(App.Db!, book);
        ShowDetails(null);
        Update();
    }

    [RelayCommand]
    private void AddBook()
    {
        Book book = new()
        {
            ISBN = "",
            Title = "",
            Author = "",
            Description = "",
            Borrower = null,
        };

        Librarian.AddBook(App.Db!, book);
        ShowDetails(App.Db.Books.Count - 1);
        Update();
    }

    [RelayCommand]
    private void SaveBook()
    {
        UpdatePanel();
        Update();
    }

    [RelayCommand]
    public void ShowDetails(int? id)
    {
        UpdatePanel();

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

        var book = App.Db.GetBookByIndex((int)id)!;
        CanRemove = book.Borrower == null;
        TitleBind = $"{book.Title}";
        AuthorBind = $"{book.Author}";
        IsbnBind = $"{book.ISBN}";
        BookDisc = $"{book.Description}";
    }

    private ObservableCollection<ViewModelBase> MapBooks(List<Book> books)
    {
        ObservableCollection<ViewModelBase> mappedBooks = [];

        foreach (Book book in books)
        {
            LibrarianBookViewModel bookModel = new(book);
            mappedBooks.Add(bookModel);
        }
        return mappedBooks;
    }

    private void UpdatePanel()
    {
        var detailedBook = App.Db.GetBookByIndex(lastDetailedBook);
        if (detailedBook == null)
        {
            return;
        }

        if (AuthorBind != null) detailedBook.Author = AuthorBind;
        if (TitleBind != null) detailedBook.Title = TitleBind;
        if (IsbnBind != null) detailedBook.ISBN = IsbnBind;
        if (BookDisc != null) detailedBook.Description = BookDisc;
    }
}
