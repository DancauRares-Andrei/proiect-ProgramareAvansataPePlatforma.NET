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
            var books = db.Books.Where(b => b.Stock > 0).ToList();
            ViewBag.Books = books;
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");
            var orders = new List<OrderViewModel>();
            var now = DateTime.Now;
            var startCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var startLastMonth = startCurrentMonth.AddMonths(-1);
            var startTwoMonthsAgo = startCurrentMonth.AddMonths(-2);

            var salesData = new List<dynamic>
    {
        new { Month = "Acum 2 luni", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startTwoMonthsAgo && od.Order.OrderDate < startLastMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 },
        new { Month = "Luna trecută", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startLastMonth && od.Order.OrderDate < startCurrentMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 },
        new { Month = "Luna curentă", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startCurrentMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 }
    };
            ViewBag.SalesData = salesData;
            try
            {
                orders = (from o in db.Orders
                          join od in db.OrderDetails on o.OrderId equals od.OrderId
                          join b in db.Books on od.BookId equals b.BookId
                          join u in db.Users on o.UserId equals u.Id
                          orderby o.OrderDate descending
                          group new { o, b, u, od } by new { o.OrderId, o.OrderDate, o.UserId, u.Email } into g
                          select new
                          {
                              g.Key.UserId,
                              g.Key.Email,
                              OrderDate = g.Key.OrderDate,
                              Books = g.Select(x => new { x.b.Title, x.od.Quantity }).ToList()
                          }).AsEnumerable() // Trecem la evaluare in memorie
                          .Select(g => new OrderViewModel
                          {
                              UserId = g.UserId,
                              UserEmail = g.Email,
                              BookDetails = g.Books.ToDictionary(x => x.Title, x => x.Quantity),
                              OrderDate = g.OrderDate
                          }).ToList();
            }
            catch (Exception)
            {
                orders = new List<OrderViewModel>();
            }
            finally
            {
                ViewBag.Orders = orders;
            }
            return View();
        }

        public ActionResult Chart()
        {
            var now = DateTime.Now;
            var startCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var startLastMonth = startCurrentMonth.AddMonths(-1);
            var startTwoMonthsAgo = startCurrentMonth.AddMonths(-2);

            var salesData = new List<dynamic>
    {
        new { Month = "Acum 2 luni", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startTwoMonthsAgo && od.Order.OrderDate < startLastMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 },
        new { Month = "Luna trecută", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startLastMonth && od.Order.OrderDate < startCurrentMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 },
        new { Month = "Luna curentă", Count = db.OrderDetails
            .Where(od => od.Order.OrderDate >= startCurrentMonth)
            .Sum(od => (int?)od.Quantity) ?? 0 }
    };

            var chart = new System.Web.Helpers.Chart(width: 600, height: 400)
                .AddTitle("Cărți Vândute în Ultimele 3 Luni")
                .AddSeries(
                    chartType: "Column",
                    xValue: salesData.Select(s => s.Month).ToArray(),
                    yValues: salesData.Select(s => s.Count).ToArray()
                )
                .GetBytes("png");

            return File(chart, "image/png");
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