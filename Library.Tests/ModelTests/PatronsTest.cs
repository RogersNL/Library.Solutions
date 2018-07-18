using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class PatronTests : IDisposable
  {
    public void Dispose()
    {
      Patron.DeleteAll();
      Book.DeleteAll();
    }
    public PatronTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    [TestMethod]
    public void Save_GetAllPatrons_Test()
    {
      //Arrange
      Patron newPatron = new Patron("Jeffrey Archer");
      Patron newPatron1 = new Patron("JK Rowling");
      newPatron.Save();
      newPatron1.Save();

      //Act
      List<Patron> expectedResult = new List<Patron>{newPatron, newPatron1};
      List<Patron> result = Patron.GetAllPatrons();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Find_Test()
    {
      //Arrange
      Patron newPatron = new Patron("Christopher Nolan");
      newPatron.Save();

      //Act
      Patron result = Patron.Find(newPatron.GetPatronId());

      //Assert
      Assert.AreEqual(newPatron, result);
    }
    [TestMethod]
    public void GetBooks_Test()
    {
      //Arrange
      Patron newPatron = new Patron("Paulo Coelho");
      newPatron.Save();
      Book newBook = new Book("The Future is Mine", 8);
      newBook.Save();
      Book newBook1 = new Book("I Don't Remember", 3);
      newBook1.Save();

      //Act
      newPatron.AddBook(newBook);
      newPatron.AddBook(newBook1);

      List<Book> expectedResult = new List<Book>{newBook, newBook1};
      List<Book> result = newPatron.GetBooks();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Edit_Test()
    {
      //Arrange
      Patron newPatron = new Patron("George R. Martin");
      newPatron.Save();
      Patron expectedPatron = new Patron("George R. R. Martin", newPatron.GetPatronId());
      //Act
      newPatron.Edit("George R. R. Martin");

      //Assert
      Assert.AreEqual(expectedPatron, newPatron);
    }
    [TestMethod]
    public void Delete_Test()
    {
      //Arrange
      Patron newPatron = new Patron("Sydney Sheldon");
      newPatron.Save();
      Patron newPatron1 = new Patron("Doomsday Conspiracy");
      newPatron1.Save();

      //Act
      newPatron.Delete();
      List<Patron> result = Patron.GetAllPatrons();
      List<Patron> expectedResult = new List<Patron>{newPatron1};
      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
  }
}
