using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyAvaloniaApp.Models;

public class Database : IDatabase
{
    [JsonPropertyName("users")]
    public List<User> Users { get; set; } = [];

    [JsonPropertyName("books")]
    public List<Book> Books { get; set; } = [];

    public List<Book> GetBooks()
    {
        return Books;
    }

    public Book? GetBook(string isbn)
    {
        List<Book> books = GetBooks();
        foreach (Book book in books)
        {
            if (book.ISBN == isbn)
            {
                return book;
            }
        }
        return null;
    }

    public Book? GetBookByIndex(int index)
    {
        List<Book> books = GetBooks();
        if (index < 0 || index >= books.Count)
        {
            return null;
        }
        return books[index];
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public bool RemoveBook(Book book)
    {
        return Books.Remove(book);
    }
}