using System.Collections.Generic;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Query
{
    public static List<Book> GetBooks(Database db)
    {
        return db.Books;
    }

    public static Book? GetBook(Database db, string isbn)
    {
        List<Book> books = GetBooks(db);
        foreach (Book book in books)
        {
            if (book.ISBN == isbn)
            {
                return book;
            }
        }
        return null;
    }

    public static bool BorrowBooks(Database db, string[] isbns, string username)
    {
        // Process all books and return failure if at least one of them failed
        bool failed = false;
        foreach (string isbn in isbns)
        {
            if (BorrowBook(db, isbn, username))
            {
                failed = true;
            }
        }
        return failed;
    }

    public static bool BorrowBook(Database db, string isbn, string username)
    {
        Book? book = GetBook(db, isbn);
        if (book == null)
        {
            return false;
        }
        if (book.Borrower != username)
        {
            return false;
        }
        
        book.Borrower = username;
        return true;
    }

    public static bool ReturnBook(Database db, string isbn, string username)
    {
        Book? book = GetBook(db, isbn);
        if (book == null)
        {
            return false;
        }
        if (book.Borrower != username)
        {
            return false;
        }

        book.Borrower = null;
        return true;
    }

    public static List<Book> GetBorrowedBooks(Database db)
    {
        List<Book> books = GetBooks(db);
        List<Book> borrowedBooks = new();
        foreach (Book book in books)
        {
            if (book.Borrower != null)
            {
                borrowedBooks.Add(book);
            }
        }
        return borrowedBooks;
    }

    public static List<Book> GetBorrowedBooksByUsername(Database db, string username)
    {
        List<Book> books = GetBooks(db);
        List<Book> borrowedBooks = new();
        foreach (Book book in books)
        {
            if (book.Borrower == username)
            {
                borrowedBooks.Add(book);
            }
        }
        return borrowedBooks;
    }
}