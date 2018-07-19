using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class BooksController : Controller
  {

    [HttpGet("/Books")]
    public ActionResult Index()
    {
        List<Book> allBook = Book.GetAllBooks();
        return View(allBook);
    }

    [HttpGet("/Books/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/Books")]
    public ActionResult Create()
    {
      Book newBook = new Book(Request.Form["newbook"], int.Parse(Request.Form["newcopies"]));
      newBook.Save();
      return RedirectToAction("Success", "Home");
    }
    [HttpGet("/Books/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Book thisBook = Book.Find(id);
      return View(thisBook);
    }
    [HttpPost("/Books/{id}/update")]
    public ActionResult Update(int id)
    {
      Book thisBook = Book.Find(id);
      thisBook.Edit(Request.Form["updatebook"], int.Parse(Request.Form["updatecopies"]));
      return RedirectToAction("Index");
    }

    [HttpGet("/Books/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Book thisBook = Book.Find(id);
      thisBook.Delete();
      return RedirectToAction("Index");
    }
    // [HttpPost("/Books/delete")]
    // public ActionResult DeleteAll()
    // {
    //   Book.ClearAll();
    //   return View();
    // }
    [HttpGet("/Books/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book selectedBook = Book.Find(id);
      List<Author> BookAuthors = selectedBook.GetAuthors();
      List<Author> allAuthors = Author.GetAllAuthors();
      model.Add("selectedBook", selectedBook);
      model.Add("BookAuthors", BookAuthors);
      model.Add("allAuthors", allAuthors);
      List<Patron> BookPatrons = selectedBook.GetPatrons();
      List<Patron> allPatrons = Patron.GetAllPatrons();
      model.Add("BookPatrons", BookPatrons);
      model.Add("allPatrons", allPatrons);
      return View(model);
    }
    [HttpPost("/Books/{BookId}/books/new")]
    public ActionResult AddBook(int BookId)
    {
      Book Book= Book.Find(BookId);
      Author author = Author.Find(int.Parse(Request.Form["authorid"]));
      Book.AddAuthor(author);
      return RedirectToAction("Details",  new { id = BookId });
    }
  }
}
