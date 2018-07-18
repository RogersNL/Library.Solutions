using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Controllers;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class BooksControllerTest
  {
    [TestMethod]
    public void Index_ReturnsCorrectView_True()
    {
      //Arrange
      BooksController controller = new BooksController();

      //Act
      ActionResult indexView = controller.Index();

      //Assert
      Assert.IsInstanceOfType(indexView, typeof(ViewResult));
    }
    [TestMethod]
    public void Index_HasCorrectModelType_BookList()
    {
      //Arrange
      BooksController controller = new BooksController();
      IActionResult actionResult = controller.Index();
      ViewResult indexView = controller.Index() as ViewResult;

      //Act
      var result = indexView.ViewData.Model;

      //Assert
      Assert.IsInstanceOfType(result, typeof(List<Book>));
    }
    [TestMethod]
    public void Details_ReturnsCorrectView_True()
    {
      //Arrange
      BooksController controller = new BooksController();

      //Act
      ActionResult detailView = controller.Details(1);

      //Assert
      Assert.IsInstanceOfType(detailView, typeof(ViewResult));
    }
    [TestMethod]
    public void Details_HasCorrectModelType_BookList()
    {
      //Arrange
      BooksController controller = new BooksController();
      IActionResult actionResult = controller.Details(1);
      ViewResult detailView = controller.Details(1) as ViewResult;

      //Act
      var result = detailView.ViewData.Model;

      //Assert
      Assert.IsInstanceOfType(result, typeof(Dictionary<string, object>));
    }
    [TestMethod]
    public void CreateForm_ReturnsCorrectView_True()
    {
      //Arrange
      BooksController controller = new BooksController();

      //Act
      ActionResult createView = controller.CreateForm();

      //Assert
      Assert.IsInstanceOfType(createView, typeof(ViewResult));
    }

    [TestMethod]
    public void UpdateForm_ReturnsCorrectView_True()
    {
      //Arrange
      BooksController controller = new BooksController();

      //Act
      ActionResult updateView = controller.UpdateForm(1);

      //Assert
      Assert.IsInstanceOfType(updateView, typeof(ViewResult));
    }
  }
}
