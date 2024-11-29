using Microsoft.AspNet.Identity;
using proiect_ProgramareAvansataPePlatforma.NET.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect_ProgramareAvansataPePlatforma.NET.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders/Create
        public ActionResult Create()
        {
            var books = db.Books.Where(b => b.Stock > 0).ToList();
            ViewBag.Books = books;
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,UserId,BookId,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Verifică dacă există suficiente stocuri
                var book = db.Books.SingleOrDefault(b => b.BookId == order.BookId);
                if (book != null)
                {
                    if (book.Stock > 0)
                    {
                        // Scade cantitatea din stoc
                        book.Stock--;

                        // Salvează modificările pentru stoc
                        db.Entry(book).State = EntityState.Modified;

                        // Salvează comanda
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Stoc insuficient pentru a finaliza cumpărarea.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Cartea nu a fost găsită.");
                }
            }

            ViewBag.BookId = new SelectList(db.Books.Where(b => b.Stock > 0), "BookId", "Title", order.BookId);
            ViewBag.UserId = User.Identity.Name; 
            return View(order);
        }

    }
}