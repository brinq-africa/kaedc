using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kaedc.Controllers
{
    public class ProductReportController : Controller
    {
        private Kaedc _context;

        public ProductReportController(Kaedc context)
        {
            _context = context;
        }
        // GET: ProductReport
        public ActionResult ProductReport()
        {
            var productreport = new ProductReportModel();
            var startDate = DateTime.Today.AddDays(-5);
            var endDate = DateTime.Today.AddDays(0);
            productreport.PrepaidVolume = _context.Transaction.Where(s => s.ServiceId == 1 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.PostpaidVolume = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.CreditVolume = _context.Transaction.Where(s => s.ServiceId == 3 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.DebitVolume = _context.Transaction.Where(s => s.ServiceId == 4 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.RefundVolume = _context.Transaction.Where(s => s.ServiceId == 5 && s.Datetime < endDate && s.Datetime > startDate).Count();

            var prepaidsalelist = _context.Transaction.Where(s => s.ServiceId == 1 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList();
            foreach (var item in prepaidsalelist)
            {
                productreport.PrepaidSales += Convert.ToDecimal(item);
            }

            var postpaidsalelist = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList();
            foreach (var item in postpaidsalelist)
            {
                productreport.PostpaidSales += Convert.ToDecimal(item);
            }


            return View(productreport);
        }

        // GET: ProductReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(ProductReport));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductReport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(ProductReport));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductReport/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(ProductReport));
            }
            catch
            {
                return View();
            }
        }
    }
}