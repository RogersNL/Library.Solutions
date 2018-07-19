using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Checkout
  {
    private int _checkoutId;
    private int _book_Id;
    private int _patron_Id;
    private DateTime _checkout_Date;
    private DateTime _due_Date;
    private bool _returned;

    public Checkout(int Book_Id, int Patron_Id, DateTime Checkout_Date, DateTime Due_Date, bool Returned, int CheckoutId = 0)
    {
      _checkoutId = CheckoutId;
      _book_Id = Book_Id;
      _patron_Id = Patron_Id;
      _checkout_Date = Checkout_Date;
      _due_Date = Due_Date;
      _returned = Returned;
    }
    public int GetCheckoutId()
    {
      return _checkoutId;
    }
    public int GetBook_Id()
    {
      return _book_Id;
    }
    public int GetPatron_Id()
    {
      return _patron_Id;
    }
    public DateTime GetCheckout_Date()
    {
      return _checkout_Date;
    }
    public DateTime GetDue_Date()
    {
      return _due_Date;
    }
    public bool GetReturned()
    {
      return _returned;
    }


    public override bool Equals(System.Object otherCheckout)
    {
      if(!(otherCheckout is Checkout))
      {
        return false;
      }
      else
      {
        Checkout newCheckout = (Checkout) otherCheckout;
        bool idEquality = this.GetCheckoutId().Equals(newCheckout.GetCheckoutId());
        bool bookIdEquality = this.GetBook_Id().Equals(newCheckout.GetBook_Id());
        bool patronIdEquality = this.GetPatron_Id().Equals(newCheckout.GetPatron_Id());
        bool checkoutDateEquality = this.GetCheckout_Date().Equals(newCheckout.GetCheckout_Date());
        bool dueDateEquality = this.GetDue_Date().Equals(newCheckout.GetDue_Date());
        bool returnedEquality = this.GetReturned().Equals(newCheckout.GetReturned());

        return (idEquality && bookIdEquality && patronIdEquality && checkoutDateEquality && dueDateEquality && returnedEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetCheckoutId().GetHashCode();
    }
    public static Book FindBook(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int BookId = 0;
      string BookTitle = "";
      int BookCopies = 0;

      while(rdr.Read())
      {
        BookId = rdr.GetInt32(0);
        BookTitle = rdr.GetString(1);
        BookCopies = rdr.GetInt32(2);
      }
      Book newBook = new Book(BookTitle, BookCopies, BookId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      // return new Patron("", "", 0);
      return newBook;
    }
  }
}
