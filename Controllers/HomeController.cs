using proiect_ProgramareAvansataPePlatforma.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect_ProgramareAvansataPePlatforma.NET.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var orders = (from o in db.Orders
                          join b in db.Books on o.BookId equals b.BookId
                          orderby o.OrderDate descending
                          select new
                          {
                              o.OrderId,
                              UserEmail = o.UserId,
                              BookTitle = b.Title,
                              o.OrderDate
                          }).ToList();

            ViewBag.Orders = orders;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}