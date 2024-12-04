using proiect_ProgramareAvansataPePlatforma.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect_ProgramareAvansataPePlatforma.NET.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Title,Author,Price,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError($"Eroare la preluarea cărților: {ex.Message}");
                    return RedirectToAction("Error", "Home", new HandleErrorInfo(ex, "Books", "Index"));
                }
                finally
                {
                    if (db != null)
                    {
                        db.Dispose();
                    }
                }
                return RedirectToAction("Index","Home");
            }

            return View(book);
        }
    }

}