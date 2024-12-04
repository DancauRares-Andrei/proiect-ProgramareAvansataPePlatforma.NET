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

        public ActionResult Create()
        {
            try
            {
                var books = db.Books.Where(b => b.Stock > 0).ToList();
                ViewBag.Books = books;
                ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");

                var userEmail = User.Identity.Name;
                var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
                if (user != null)
                {
                    ViewBag.UserId = user.Id;
                }
                else
                {
                    ViewBag.UserId = null; 
                }

                return View();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Eroare la inițializarea paginii Create: {ex.Message}");
                return RedirectToAction("Error", "Home", new HandleErrorInfo(ex, "Books", "Create"));
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, int[] selectedBooks, int[] quantities)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    order.OrderDate = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Eroare la crearea comenzii: {ex.Message}");
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }

            var books = db.Books.Where(b => b.Stock > 0).ToList();
            ViewBag.Books = books;
            ViewBag.UserId = new SelectList(db.Users, "Id", "Id");
            return View(order);
        }


    }
}