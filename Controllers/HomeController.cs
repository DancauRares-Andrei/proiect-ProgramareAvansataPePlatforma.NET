﻿using proiect_ProgramareAvansataPePlatforma.NET.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                var books = db.Books.Where(b => b.Stock > 0).ToList();
                ViewBag.Books = books;
                ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");

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

                var orders = new List<OrderViewModel>();

                try
                {
                    orders = (from o in db.Orders
                              join od in db.OrderDetails on o.OrderId equals od.OrderId
                              join u in db.Users on o.UserId equals u.Id
                              group new { o, u, od } by new { o.OrderId, o.OrderDate, o.UserId, u.Email } into g
                              orderby g.Key.OrderDate descending
                              select new
                              {
                                  g.Key.UserId,
                                  g.Key.Email,
                                  OrderDate = g.Key.OrderDate,
                                  Books = g.Select(x => new { x.od.BookTitle, x.od.Quantity }).ToList()
                              }).AsEnumerable() // Trecem la evaluare in memorie
               .Select(g => new OrderViewModel
               {
                   UserId = g.UserId,
                   UserEmail = g.Email,
                   BookDetails = g.Books.ToDictionary(x => x.BookTitle, x => x.Quantity),
                   OrderDate = g.OrderDate
               }).ToList();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError($"Eroare la preluarea comenzilor: {ex.Message}");
                    orders = new List<OrderViewModel>();
                }
                finally
                {
                    ViewBag.Orders = orders;
                }

                return View();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Eroare la încărcarea paginii Index: {ex.Message}");
                return RedirectToAction("Error", "Home", new HandleErrorInfo(ex, "Books", "Index"));
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }


        public ActionResult Chart()
        {
            try
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
                    .AddSeries(
                        chartType: "Column",
                        xValue: salesData.Select(s => s.Month).ToArray(),
                        yValues: salesData.Select(s => s.Count).ToArray()
                    )
                    .GetBytes("png");

                return File(chart, "image/png");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"Eroare la generarea graficului: {ex.Message}");
                return RedirectToAction("Error", "Home", new HandleErrorInfo(ex, "Books", "Chart"));
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
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
        public ActionResult Error(ErrorViewModel model)
        {
            return View(model);
        }
        public ActionResult ShowReport()
        {
            CrystalReport1 rpt = new CrystalReport1();
            rpt.Load();
            Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(s, "application/pdf");
        }
    }
}