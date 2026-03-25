using System.Collections.Generic;
using System.Data.Common;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Member
{
    public static bool BorrowBooks(IDatabase db, string[] isbns, string username)
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

    public static bool BorrowBook(IDatabase db, string isbn, string username)
    {
        Book? book = db.GetBook(isbn);
        if (book == null)
        {
            return false;
        }
        if (book.Borrower != null)
        {
            return false;
        }

        book.Borrower = username;
        return true;
    }

    public static bool ReturnBook(IDatabase db, string isbn, string username)
    {
        Book? book = db.GetBook(isbn);
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

    public static List<Book> GetBorrowedBooksByUsername(IDatabase db, string username)
    {
        List<Book> books = db.GetBooks();
        List<Book> borrowedBooks = [];
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