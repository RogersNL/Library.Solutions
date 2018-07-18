using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTests : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
    public AuthorTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    [TestMethod]
    public void Save_GetAllAuthors_Test()
    {
      //Arrange
      Author newAuthor = new Author("Jeffrey Archer");
      Author newAuthor1 = new Author("JK Rowling");
      newAuthor.Save();
      newAuthor1.Save();

      //Act
      List<Author> expectedResult = new List<Author>{newAuthor, newAuthor1};
      List<Author> result = Author.GetAllAuthors();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Find_Test()
    {
      //Arrange
      Author newAuthor = new Author("Christopher Nolan");
      newAuthor.Save();

      //Act
      Author result = Author.Find(newAuthor.GetAuthorId());

      //Assert
      Assert.AreEqual(newAuthor, result);
    }
    [TestMethod]
    public void GetBooks_Test()
    {
      //Arrange
      Author newAuthor = new Author("Paulo Coelho");
      newAuthor.Save();
      Book newBook = new Book("The Future is Mine", 8);
      newBook.Save();
      Book newBook1 = new Book("I Don't Remember", 4);
      newBook1.Save();

      //Act
      newAuthor.AddBook(newBook);
      newAuthor.AddBook(newBook1);

      List<Book> expectedResult = new List<Book>{newBook, newBook1};
      List<Book> result = newAuthor.GetBooks();

      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
    [TestMethod]
    public void Edit_Test()
    {
      //Arrange
      Author newAuthor = new Author("George R. Martin");
      newAuthor.Save();
      Author expectedAuthor = new Author("George R. R. Martin", newAuthor.GetAuthorId());
      //Act
      newAuthor.Edit("George R. R. Martin");

      //Assert
      Assert.AreEqual(expectedAuthor, newAuthor);
    }
    [TestMethod]
    public void Delete_Test()
    {
      //Arrange
      Author newAuthor = new Author("Sydney Sheldon");
      newAuthor.Save();
      Author newAuthor1 = new Author("Doomsday Conspiracy");
      newAuthor1.Save();

      //Act
      newAuthor.Delete();
      List<Author> result = Author.GetAllAuthors();
      List<Author> expectedResult = new List<Author>{newAuthor1};
      //Assert
      CollectionAssert.AreEqual(expectedResult, result);
    }
  }
}
