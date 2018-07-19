using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Controllers;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class PatronsControllerTest
  {
    [TestMethod]
    public void Index_ReturnsCorrectView_True()
    {
      //Arrange
      PatronsController controller = new PatronsController();

      //Act
      ActionResult indexView = controller.Index();

      //Assert
      Assert.IsInstanceOfType(indexView, typeof(ViewResult));
    }
    [TestMethod]
    public void Index_HasCorrectModelType_PatronList()
    {
      //Arrange
      PatronsController controller = new PatronsController();
      IActionResult actionResult = controller.Index();
      ViewResult indexView = controller.Index() as ViewResult;

      //Act
      var result = indexView.ViewData.Model;

      //Assert
      Assert.IsInstanceOfType(result, typeof(List<Patron>));
    }
    [TestMethod]
    public void Details_ReturnsCorrectView_True()
    {
      //Arrange
      PatronsController controller = new PatronsController();

      //Act
      ActionResult detailView = controller.Details(1);

      //Assert
      Assert.IsInstanceOfType(detailView, typeof(ViewResult));
    }
    [TestMethod]
    public void Details_HasCorrectModelType_PatronList()
    {
      //Arrange
      PatronsController controller = new PatronsController();
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
      PatronsController controller = new PatronsController();

      //Act
      ActionResult createView = controller.CreateForm();

      //Assert
      Assert.IsInstanceOfType(createView, typeof(ViewResult));
    }

    [TestMethod]
    public void UpdateForm_ReturnsCorrectView_True()
    {
      //Arrange
      PatronsController controller = new PatronsController();

      //Act
      ActionResult updateView = controller.UpdateForm(1);

      //Assert
      Assert.IsInstanceOfType(updateView, typeof(ViewResult));
    }
  }
}
