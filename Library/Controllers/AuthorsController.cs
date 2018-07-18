using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {

    [HttpGet("/authors")]
    public ActionResult Index()
    {
        List<Author> allAuthor = Author.GetAllAuthors();
        return View(allAuthor);
    }

    [HttpGet("/authors/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/authors")]
    public ActionResult Create()
    {
      Author newAuthor = new Author(Request.Form["newenrolldate"]);
      newAuthor.Save();
      return RedirectToAction("Success", "Home");
    }
    [HttpGet("/authors/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
      Author thisAuthor = Author.Find(id);
      return View(thisAuthor);
    }
    [HttpPost("/authors/{id}/update")]
    public ActionResult Update(int id)
    {
      Author thisAuthor = Author.Find(id);
      thisAuthor.Edit(Request.Form["updateenrolldate"]);
      return RedirectToAction("Index");
    }

    [HttpGet("/authors/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Author thisAuthor = Author.Find(id);
      thisAuthor.Delete();
      return RedirectToAction("Index");
    }
    // [HttpPost("/authors/delete")]
    // public ActionResult DeleteAll()
    // {
    //   Author.ClearAll();
    //   return View();
    // }
    [HttpGet("/authors/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Author selectedAuthor = Author.Find(id);
      List<Book> authorBooks = selectedAuthor.GetBooks();
      List<Book> allBooks = Book.GetAllBooks();
      model.Add("selectedAuthor", selectedAuthor);
      model.Add("authorBooks", authorBooks);
      model.Add("allBooks", allBooks);
      return View(model);
    }
    [HttpPost("/authors/{authorId}/books/new")]
    public ActionResult AddBook(int authorId)
    {
      Author author= Author.Find(authorId);
      Book book = Book.Find(int.Parse(Request.Form["bookid"]));
      author.AddBook(book);
      return RedirectToAction("Details",  new { id = authorId });
    }
  }
}
