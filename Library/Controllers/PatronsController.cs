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
      Patron newPatron = new Patron(Request.Form["newenrolldate"]);
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
      thisPatron.Edit(Request.Form["updateenrolldate"]);
      return RedirectToAction("Index");
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
      model.Add("selectedPatron", selectedPatron);
      model.Add("patronBooks", patronBooks);
      model.Add("allBooks", allBooks);
      return View(model);
    }
    [HttpPost("/patrons/{patronId}/books/new")]
    public ActionResult AddBook(int patronId)
    {
      Patron patron= Patron.Find(patronId);
      Book book = Book.Find(int.Parse(Request.Form["bookid"]), Convert.ToDateTime(Request.Form["checkoutdate"]), Convert.ToDateTime(Request.Form["duedate"]));
      patron.AddBook(book);
      return RedirectToAction("Details",  new { id = patronId });
    }
  }
}
