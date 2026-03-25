using System.Collections.Generic;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Librarian
{
    public static List<Book> GetBorrowedBooks(IDatabase db)
    {
        List<Book> books = db.GetBooks();
        List<Book> borrowedBooks = [];
        foreach (Book book in books)
        {
            if (book.Borrower != null)
            {
                borrowedBooks.Add(book);
            }
        }
        return borrowedBooks;
    }

    public static bool AddBook(IDatabase db, Book book)
    {
        var existingBook = db.GetBook(book.ISBN);
        if (existingBook != null)
        {
            return false;
        }

        db.AddBook(book);
        return true;
    }

    public static bool RemoveBook(IDatabase db, Book book)
    {
        return db.RemoveBook(book);
    }
}