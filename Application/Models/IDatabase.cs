using System.Collections.Generic;
using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Models;

public interface IDatabase
{
    List<Book> GetBooks();
    Book? GetBook(string isbn);
    Book? GetBookByIndex(int index);
    void AddBook(Book book);
    bool RemoveBook(Book book);
}