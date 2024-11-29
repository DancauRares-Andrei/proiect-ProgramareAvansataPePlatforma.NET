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

            // Obține Id-ul utilizatorului pe baza email-ului User.Identity.Name
            var userEmail = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user != null)
            {
                ViewBag.UserId = user.Id;
            }
            else
            {
                // Dacă utilizatorul nu este găsit, gestionează situația corespunzător
                ViewBag.UserId = null; // Sau orice valoare implicită dorești
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, int[] selectedBooks, int[] quantities)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();

                // Adăugare OrderDetails
                for (int i = 0; i < selectedBooks.Length; i++)
                {
                    int bookId = selectedBooks[i];
                    int quantity = quantities[i];
                    var book = db.Books.Find(bookId);
                    if (book != null && book.Stock >= quantity)
                    {
                        book.Stock -= quantity;
                        db.Entry(book).State = EntityState.Modified;

                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            BookId = bookId,
                            Quantity = quantity
                        };
                        db.OrderDetails.Add(orderDetail);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            var books = db.Books.Where(b => b.Stock > 0).ToList();
            ViewBag.Books = books;
            ViewBag.UserId = new SelectList(db.Users, "Id", "Id");
            return View(order);
        }

    }
}