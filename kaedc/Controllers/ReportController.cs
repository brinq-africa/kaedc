using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kaedc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kaedc.Controllers
{
    public class ReportController : Controller
    {
        private Kaedc _context;

        public ReportController(Kaedc context)
        {
            _context = context;
        }


        // GET: ProductReport
        //[Route("productreport")]
        public ActionResult ProductReport()
        {
            var productreport = new ProductReportModel();
            var startDate = DateTime.Today.AddDays(-40);
            var endDate = DateTime.Today.AddDays(0);
            var trans = _context.Transaction.ToList();

            productreport.PrepaidVolume = _context.Transaction.Where(s => s.ServiceId == 1 && (s.Datetime >= startDate) && (s.Datetime < endDate)).Count();
            productreport.PostpaidVolume = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.CreditVolume = _context.Transaction.Where(s => s.ServiceId == 3 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.DebitVolume = _context.Transaction.Where(s => s.ServiceId == 4 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.RefundVolume = _context.Transaction.Where(s => s.ServiceId == 5 && s.Datetime < endDate && s.Datetime > startDate).Count();
            productreport.TotalVolume = _context.Transaction.Where(s => s.ServiceId != 6 && s.Datetime < endDate && s.Datetime > startDate).Count();

            productreport.PrepaidSales = _context.Transaction.Where(s => s.ServiceId == 1 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();
            productreport.PostpaidSales = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();
            productreport.CreditSales = _context.Transaction.Where(s => s.ServiceId == 3 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();
            productreport.DebitSales = _context.Transaction.Where(s => s.ServiceId == 4 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();
            productreport.RefundSales = _context.Transaction.Where(s => s.ServiceId == 5 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();
            productreport.TotalSales = _context.Transaction.Where(s => s.ServiceId != 6 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.Amount).ToList().Sum();

            productreport.BrinqPrepaidProfit = _context.Transaction.Where(s => s.ServiceId == 1 && (s.Datetime >= startDate) && (s.Datetime < endDate)).Select(s => s.AgentProfit).ToList().Sum();
            productreport.BrinqPostpaidProfit = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.AgentProfit).ToList().Sum();
            productreport.BrinqTotalProfit = productreport.BrinqPostpaidProfit + productreport.BrinqPrepaidProfit;

            productreport.AgentPrepaidProfit = _context.Transaction.Where(s => s.ServiceId == 1 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.AgentProfit).ToList().Sum();
            productreport.AgentPostpaidProfit = _context.Transaction.Where(s => s.ServiceId == 2 && s.Datetime < endDate && s.Datetime > startDate).Select(s => s.AgentProfit).ToList().Sum();
            productreport.AgentTotalProfit = productreport.AgentPostpaidProfit + productreport.AgentPrepaidProfit;

            List<AgentReportModel> agentreport = new List<AgentReportModel>();

            foreach (var user in _context.Kaedcuser.Where(u => u.IsActive == 1))
            {
                var c = _context.Kaedcuser.Where(u => u.Id == user.Id).Select(u => u.Transaction.Where(t => t.ServiceId == 1)).Count();
            }
            

            return View(productreport);
        }

        public ActionResult AgentReport()
        {
            
            List<AgentReportModel> agentreport = new List<AgentReportModel>();  
            
            var startDate = DateTime.Today.AddDays(-40);
            var endDate = DateTime.Today.AddDays(0);

            foreach (var user in _context.Kaedcuser.Where(u => u.IsActive == 1))
            {
                var agent = new AgentReportModel();
                agent.Fullname = user.Firstname + " " + user.Surname;
                agent.TotalSales = _context.Transaction.Where(t => t.KaedcUserNavigation.Id == user.Id).Select(t => t.Amount).Sum();
                agent.PrepaidTotalSales = _context.Transaction.Where(t => t.KaedcUserNavigation.Id == user.Id && t.ServiceId == 1).Select(t => t.Amount).ToList().Sum();
                agent.PostpaidTotalSales = _context.Transaction.Where(t => t.KaedcUserNavigation.Id == user.Id && t.ServiceId == 2).Select(t => t.Amount).ToList().Sum();
                agent.PrepaidTotalProfit = _context.Transaction.Where(t => t.KaedcUserNavigation.Id == user.Id && t.ServiceId == 1).Select(t => t.AgentProfit).ToList().Sum();
                agent.PostpaidTotalProfit = _context.Transaction.Where(t => t.KaedcUserNavigation.Id == user.Id && t.ServiceId == 2).Select(t => t.AgentProfit).ToList().Sum(); ;
                agent.Location = user.CompanyAddress;

                agentreport.Add(agent);
            }

            return View(agentreport);
        }
    }
}