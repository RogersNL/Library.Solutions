using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class BookTests : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
      Patron.DeleteAll();
    }
    public BookTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    [TestMethod]
    public void Save_GetAllBooks_Test()
    {
      //Arrange
      Book newBook = new Book("The Alchemist", 3);
      Book newBook1 = new Book("The Pilgrimage", 5);
      newBook.Save();
      newBook1.Save();

      //Act
      List<Book> expectedResult = new List<Book>{newBook, newBook1};
      List<Book> result = Book.GetAllBooks();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Find_Test()
    {
      //Arrange
      Book newBook = new Book("Harry Potter", 9);
      newBook.Save();

      //Act
      Book result = Book.Find(newBook.GetBookId());

      //Assert
      Assert.AreEqual(newBook, result);
    }
    [TestMethod]
    public void GetAuthor_Test()
    {
      //Arrange
      Book newBook = new Book("Twilight", 8);
      newBook.Save();
      Author newAuthor = new Author("Nicholas Rogers");
      newAuthor.Save();
      Author newAuthor1 = new Author("Swati Sahay");
      newAuthor1.Save();

      //Act
      newBook.AddAuthor(newAuthor);
      newBook.AddAuthor(newAuthor1);

      List<Author> expectedResult = new List<Author>{newAuthor, newAuthor1};
      List<Author> result = newBook.GetAuthors();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void GetPatron_Test()
    {
      //Arrange
      Book newBook = new Book("Twilight", 6);
      newBook.Save();
      Patron newPatron = new Patron("Sonny");
      newPatron.Save();
      Patron newPatron1 = new Patron("Jean");
      newPatron1.Save();

      //Act
      newBook.AddPatron(newPatron);
      newBook.AddPatron(newPatron1);

      List<Patron> expectedResult = new List<Patron>{newPatron, newPatron1};
      List<Patron> result = newBook.GetPatrons();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Edit_Test()
    {
      //Arrange
      Book newBook = new Book("Lord of the Rings", 2);
      newBook.Save();
      Book expectedBook = new Book("DragonFlyLord of the Rings", 2, newBook.GetBookId());
      //Act
      newBook.Edit("DragonFlyLord of the Rings", 2);

      //Assert
      Assert.AreEqual(expectedBook, newBook);
    }
    [TestMethod]
    public void Delete_Test()
    {
      //Arrange
      Book newBook = new Book("Uh", 4);
      newBook.Save();
      Book newBook1 = new Book("Haha", 5);
      newBook1.Save();

      //Act
      newBook.Delete();
      List<Book> result = Book.GetAllBooks();
      List<Book> expectedResult = new List<Book>{newBook1};
      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
  }
}
