using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyAvaloniaApp.Domain;
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

    public CatalogViewModel()
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
            books = Member.GetBorrowedBooksByUsername(App.Db!, App.CurrentUser?.Username!);
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

        var book = App.Db.GetBookByIndex((int)id);
        if (book == null)
        {
            return;
        }

        BookDisc =
            $"{book.Title}\n" +
            $"From: {book.Author}\n" +
            $"ISBN: {book.ISBN}\n\n" +
            $"Description: {book.Description}\n";
    }

    private ObservableCollection<ViewModelBase> MapBooks(List<Book> books)
    {
        ObservableCollection<ViewModelBase> mappedBooks = [];

        foreach (Book book in books)
        {
            BookViewModel bookModel = new(book);
            bookModel.BorrowStateChanged += OnBorrowStateChanged;
            mappedBooks.Add(bookModel);
        }
        return mappedBooks;
    }

    private void OnBorrowStateChanged(object? sender, EventArgs e)
    {
        Update();
    }
}
