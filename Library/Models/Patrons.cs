using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    private int _patronId;
    private string _patronName;

    public Patron(string PatronName, int PatronId = 0)
    {
      _patronId = PatronId;
      _patronName = PatronName;
    }
    public int GetPatronId()
    {
      return _patronId;
    }
    public string GetPatronName()
    {
      return _patronName;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if(!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = this.GetPatronId().Equals(newPatron.GetPatronId());
        bool nameEquality = this.GetPatronName().Equals(newPatron.GetPatronName());
        return (idEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetPatronId().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@patronName);";

      cmd.Parameters.Add(new MySqlParameter("@patronName", _patronName));

      cmd.ExecuteNonQuery();
      _patronId = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Patron> GetAllPatrons()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int PatronId = rdr.GetInt32(0);
        string PatronName = rdr.GetString(1);
        Patron newPatron = new Patron(PatronName, PatronId);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
      // return new List<Patron>{};
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM patrons; DELETE FROM checkouts;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int PatronId = 0;
      string PatronName = "";

      while(rdr.Read())
      {
        PatronId = rdr.GetInt32(0);
        PatronName = rdr.GetString(1);
      }
      Patron newPatron = new Patron(PatronName, PatronId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // return new Patron("", "", 0);
      return newPatron;
    }
    public void AddBook(Book newBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts (patron_id, book_id, checkout_date, due_date, returned) VALUES (@PatronId, @BookId, @TodayDate, @DueDate, @Returned);";

      cmd.Parameters.Add(new MySqlParameter("@PatronId", _patronId));
      cmd.Parameters.Add(new MySqlParameter("@BookId", newBook.GetBookId()));
      cmd.Parameters.Add(new MySqlParameter("@TodayDate", DateTime.Now.Date.ToString("yyyy-MM-dd")));
      TimeSpan fourWeeks = new TimeSpan(28,0,0,0);
      cmd.Parameters.Add(new MySqlParameter("@DueDate", DateTime.Now.Add(fourWeeks).Date.ToString("yyyy-MM-dd")));
      cmd.Parameters.Add(new MySqlParameter("@Returned", false));


      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Book> GetBooks()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM patrons
      JOIN checkouts ON (patrons.id = checkouts.patron_id)
      JOIN books ON (checkouts.book_id = books.id)
      WHERE patrons.id = @PatronId;";

      cmd.Parameters.Add(new MySqlParameter("@PatronId", _patronId));

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Book> books = new List<Book>{};

      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        int bookCopies = rdr.GetInt32(2);
        Book newBook = new Book(bookTitle, bookCopies, bookId);
        books.Add(newBook);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return books;
      // return new List<Book>{};
    }
    public void Edit(string newPatronName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE patrons SET name = @newPatronName WHERE id = @searchId;";

      cmd.Parameters.Add(new MySqlParameter("@searchId", _patronId));
      cmd.Parameters.Add(new MySqlParameter("@newPatronName", newPatronName));

      cmd.ExecuteNonQuery();
      _patronName = newPatronName;

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
      cmd.CommandText = @"DELETE FROM patrons WHERE id = @PatronId; DELETE FROM checkouts WHERE patron_id = @PatronId;";

      cmd.Parameters.Add(new MySqlParameter("@PatronId", this.GetPatronId()));

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
