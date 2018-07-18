using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Book
  {
    private int _bookId;
    private string _title;
    private int _copies;

    public Book(string Title, int Copies, int BookId = 0)
    {
      _bookId = BookId;
      _title = Title;
      _copies = Copies;
    }
    public int GetBookId()
    {
      return _bookId;
    }
    public string GetTitle()
    {
      return _title;
    }
    public int GetCopies()
    {
      return _copies;
    }
    public override bool Equals(System.Object otherBook)
    {
      if(!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = this.GetBookId().Equals(newBook.GetBookId());
        bool titleEquality = this.GetTitle().Equals(newBook.GetTitle());
        bool copiesEquality = this.GetCopies().Equals(newBook.GetCopies());
        return (idEquality && titleEquality && copiesEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetBookId().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (title, copies) VALUES (@title, @copies);";

      cmd.Parameters.Add(new MySqlParameter("@title", _title));
      cmd.Parameters.Add(new MySqlParameter("@copies", _copies));

      cmd.ExecuteNonQuery();
      _bookId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Book> GetAllBooks()
    {
      List<Book> allBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int BookId = rdr.GetInt32(0);
        string Title = rdr.GetString(1);
        int Copies = rdr.GetInt32(2);
        Book newBook = new Book(Title, Copies, BookId);
        allBooks.Add(newBook);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBooks;
      // return new List<Book>{};
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books; DELETE FROM authors_books; DELETE FROM checkouts;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int BookId = 0;
      string Title = "";
      int Copies = 0;
      while(rdr.Read())
      {
        BookId = rdr.GetInt32(0);
        Title = rdr.GetString(1);
        Copies = rdr.GetInt32(2);
      }
      Book newBook = new Book(Title, Copies, BookId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // return new Book("", "", 0);
      return newBook;
    }
    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books (book_id, author_id) VALUES (@BookId, @AuthorId);";

      cmd.Parameters.Add(new MySqlParameter("@BookId", _bookId));
      cmd.Parameters.Add(new MySqlParameter("@AuthorId", newAuthor.GetAuthorId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Author> GetAuthors()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT authors.* FROM books
      JOIN authors_books ON (books.id = authors_books.book_id)
      JOIN authors ON (authors_books.author_id = authors.id)
      WHERE books.id = @BookId;";

      cmd.Parameters.Add(new MySqlParameter("@BookId", _bookId));

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        authors.Add(newAuthor);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return authors;
      // return new List<Student>{};
    }
    public void AddPatron(Patron newPatron)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts (book_id, patron_id) VALUES (@BookId, @PatronId);";

      cmd.Parameters.Add(new MySqlParameter("@BookId", _bookId));
      cmd.Parameters.Add(new MySqlParameter("@PatronId", newPatron.GetPatronId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Patron> GetPatrons()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT patrons.* FROM books
      JOIN checkouts ON (books.id = checkouts.book_id)
      JOIN patrons ON (checkouts.patron_id = patrons.id)
      WHERE books.id = @BookId;";

      cmd.Parameters.Add(new MySqlParameter("@BookId", _bookId));

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Patron> patrons = new List<Patron>{};

      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron newPatron = new Patron(patronName, patronId);
        patrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return patrons;
      // return new List<Student>{};
    }
    public void Edit(string newBookTitle, int newCopies)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE books SET title = @newBookTitle, copies = @newCopies WHERE id = @searchId;";

      cmd.Parameters.Add(new MySqlParameter("@searchId", _bookId));
      cmd.Parameters.Add(new MySqlParameter("@newBookTitle", newBookTitle));
      cmd.Parameters.Add(new MySqlParameter("@newCopies", newCopies));

      cmd.ExecuteNonQuery();
      _title = newBookTitle;


      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books WHERE id = @BookId; DELETE FROM checkouts WHERE book_id = @BookId; DELETE FROM authors_books WHERE id = @BookId;";

      cmd.Parameters.Add(new MySqlParameter("@BookId", this.GetBookId()));

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
