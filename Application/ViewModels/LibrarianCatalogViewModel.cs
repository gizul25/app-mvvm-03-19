using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private void Remove()
    {
        App.Db.Books.RemoveAt(lastDetailedBook);
        BookList = Books(App.Db.Books);
        ShowBookDetails = false;
        SpaceSize = 2;
        lastDetailedBook = -1;
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

        App.Db.Books.Add(book);
        ShowDetails(App.Db.Books.Count - 1);
        BookList = Books(App.Db.Books);
    }

    [RelayCommand]
    private void SaveBook()
    {
        if (lastDetailedBook != -1)
        {
            if (AuthorBind != null) App.Db.Books[lastDetailedBook].Author = AuthorBind;
            if (TitleBind != null) App.Db.Books[lastDetailedBook].Title = TitleBind;
            if (IsbnBind != null) App.Db.Books[lastDetailedBook].ISBN = IsbnBind;
            if (BookDisc != null) App.Db.Books[lastDetailedBook].Description = BookDisc;
        }
        BookList = Books(App.Db.Books);
    }

    [RelayCommand]
    public void ShowDetails(int? id)
    {
        if (lastDetailedBook != -1)
        {
            if (AuthorBind != null) App.Db.Books[lastDetailedBook].Author = AuthorBind;
            if (TitleBind != null) App.Db.Books[lastDetailedBook].Title = TitleBind;
            if (IsbnBind != null) App.Db.Books[lastDetailedBook].ISBN = IsbnBind;
            if (BookDisc != null) App.Db.Books[lastDetailedBook].Description = BookDisc;
        }

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

        CanRemove = App.Db.Books[(int)id].Borrower == null;
        SpaceSize = 1;
        lastDetailedBook = (int)id;
        ShowBookDetails = true;
        TitleBind = $"{App.Db.Books[(int)id].Title}";
        AuthorBind = $"{App.Db.Books[(int)id].Author}";
        IsbnBind = $"{App.Db.Books[(int)id].ISBN}";
        BookDisc = $"{App.Db.Books[(int)id].Description}";
    }

    public LibrarianCatalogViewModel()
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
            if (!onlyBorrowed || book.Borrower != null)
            {
                LibrarianBookViewModel bookModel = new(id);
                books.Add(bookModel);
            }
        }
        return books;
    }
}
