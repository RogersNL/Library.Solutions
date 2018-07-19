using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class PatronsController : Controller
  {


    [HttpGet("/patrons")]
    public ActionResult Index()
    {
        List<Patron> allPatron = Patron.GetAllPatrons();
        return View(allPatron);
    }

    [HttpGet("/patrons/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/patrons")]
    public ActionResult Create()
    {
      Patron newPatron = new Patron(Request.Form["newpatron"]);
      newPatron.Save();
      return RedirectToAction("Success", "Home");
    }
    [HttpGet("/patrons/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Patron thisPatron = Patron.Find(id);
      return View(thisPatron);
    }
    [HttpPost("/patrons/{id}/update")]
    public ActionResult Update(int id)
    {
      Patron thisPatron = Patron.Find(id);
      thisPatron.Edit(Request.Form["updatepatron"]);
      return RedirectToAction("Index");
    }
    [HttpGet("/patrons/search")]
    public ActionResult Patrons()
    {
      List<Patron> emptyList = new List<Patron>{};
      return View(emptyList);
    }
    [HttpPost("/patrons/search")]
    public ActionResult PatronSearch()
    {
       return View("Index",Patron.SearchPatrons(Request.Form["search"]));
    }

    [HttpGet("/patrons/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Patron thisPatron = Patron.Find(id);
      thisPatron.Delete();
      return RedirectToAction("Index");
    }
    // [HttpPost("/patrons/delete")]
    // public ActionResult DeleteAll()
    // {
    //   Patron.ClearAll();
    //   return View();
    // }
    [HttpGet("/patrons/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Patron selectedPatron = Patron.Find(id);
      List<Book> patronBooks = selectedPatron.GetBooks();
      List<Book> allBooks = Book.GetAllBooks();
      List<Author> allAuthors = Author.GetAllAuthors();
      List<Checkout> patronCheckouts = selectedPatron.GetCheckouts();
      model.Add("patronCheckouts", patronCheckouts);
      model.Add("allAuthors", allAuthors);
      model.Add("selectedPatron", selectedPatron);
      model.Add("patronBooks", patronBooks);
      model.Add("allBooks", allBooks);
      return View(model);
    }
    [HttpPost("/patrons/{patronId}/books/new")]
    public ActionResult AddBook(int patronId)
    {
      Patron patron= Patron.Find(patronId);
      Book book = Book.Find(int.Parse(Request.Form["bookid"]));
      patron.AddBook(book);
      return RedirectToAction("Details",  new { id = patronId });
    }
  }
}
