using MyAvaloniaApp.Models;

namespace MyAvaloniaApp.Domain;

public class Generator
{
    public static Database CreateFreshDatabase()
    {
        Database db = new();
        User user = new()
        {
            Username = "alex",
            Role = "librarian",
            PasswordHash = Auth.GenerateHashAndSalt("123123"),
        };
        db.Users.Add(user);
        User user2 = new()
        {
            Username = "steve",
            Role = "member",
            PasswordHash = Auth.GenerateHashAndSalt("321321"),
        };
        db.Users.Add(user2);

        Book book = new()
        {
            ISBN = "0575082445",
            Title = "The Last Wish",
            Author = "Andrzej Sapkowski",
            Description = "Geralt is a witcher, a man whose magic powers, enhanced by long training and a mysterious elixir, have made him a brilliant fighter and a merciless assassin. Yet he is no ordinary murderer: his targets are the multifarious monsters and vile fiends that ravage the land and attack the innocent. He roams the country seeking assignments, but gradually comes to realise that while some of his quarry are unremittingly vile, vicious grotesques, others are the victims of sin, evil or simple naivety. One reviewer said: 'This book is a sheer delight. It is beautifully written, full of vitality and endlessly inventive: its format, with half a dozen episodes and intervening rest periods for both the hero and the reader, allows for a huge range of characters, scenarios and action. It's thought-provoking without being in the least dogmatic, witty without descending to farce and packed with sword fights without being derivative. The dialogue sparkles; characters morph almost imperceptibly from semi-cliche to completely original; nothing is as it first seems. Sapkowski succeeds in seamlessly welding familiar ideas, unique settings and delicious twists of originality: his Beauty wants to rip the throat out of a sensitive Beast; his Snow White seeks vengeance on all and sundry, his elves are embittered and vindictive. It's easily one of the best things I've read in ages.'. The book has been read, but is in excellent condition. Pages are intact and not marred by notes or highlighting. The spine remains undamaged. ",
            Borrower = null,
        };
        db.Books.Add(book);
        Book book2 = new()
        {
            ISBN = "1473211549",
            Title = "Sword of Destiny",
            Author = "Andrzej Sapkowski",
            Description = "Most items will be dispatched the same or the next working day. A copy that has been read but remains in clean condition. All of the pages are intact and the cover is intact and the spine may show signs of wear. The book may have minor markings which are not specifically mentioned.",
            Borrower = null,
        };
        db.Books.Add(book2);
        Book book3 = new()
        {
            ISBN = "1399611402",
            Title = "Blood of Elves",
            Author = "Andrzej Sapkowski",
            Description = "The Witcher, Geralt of Rivia, holds the fate of the world in his hands.",
            Borrower = null,
        };
        db.Books.Add(book3);

        return db;
    }
}