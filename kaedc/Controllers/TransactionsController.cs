using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kaedc.Models;
using kaedc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace kaedc.Controllers
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class TransactionsController : Controller
    {
        private readonly Kaedc _context;

        public TransactionsController(Kaedc context)
        {
            _context = context;
        }

        public async Task<IActionResult> viewtransactions()
        {
            var kaedc = _context.Transaction.Include(t => t.PaymentMethod).Include(t => t.Service);
            return View(await kaedc.ToListAsync());
        }

        public async Task<IActionResult> pendingtransactions()
        {
            var kaedc = _context.Transaction.Where(t => t.transactionsStatus == "pending").Include(t => t.PaymentMethod).Include(t => t.Service);
            return View(await kaedc.ToListAsync());
        }

        public async Task<IActionResult> refundtransactions()
        {
            var kaedc = _context.Transaction.Where(t => t.ServiceId == 5).Include(t => t.PaymentMethod).Include(t => t.Service);
            return View(await kaedc.ToListAsync());
        }
        public IActionResult BalanceUpdated(CreditDebitBindingModel creditDebit)
        {
            ViewData["User"] = _context.Kaedcuser.Where(u => u.BrinqaccountNumber == creditDebit.BrinqAccountNumber).FirstOrDefault();
            if (creditDebit.serviceId == 3)
            {
                ViewData["Amount"] = Convert.ToDecimal(creditDebit.Amount) - 50;
                ViewData["Action"] = "Credited";
                ViewData["Verb"] = "to";
            }
            else
            {
                ViewData["Amount"] = Convert.ToDecimal(creditDebit.Amount);
                ViewData["Action"] = "Debited";
                ViewData["Verb"] = "from";
            }
            
            return View();
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var kaedc = _context.Transaction.Include(t => t.PaymentMethod).Include(t => t.Service);
            return View(await kaedc.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.PaymentMethod)
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/CreditUser
        public IActionResult CreditUser()
        {
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name");
            ViewData["CreditsAndDebits"] = _context.Transaction.Where(t => t.ServiceId == 3 && t.ServiceId == 4).Include(t => t.PaymentMethod).Include(t => t.Service);
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditUser(CreditDebitBindingModel creditDebit)
        {
            if (ModelState.IsValid)
            {

                //_context.Transaction.Add(transaction);
                //await _context.SaveChangesAsync();

                if (creditDebit.serviceId == 3)
                {
                    ExtraMethods.CreditUser(creditDebit.BrinqAccountNumber, creditDebit.Amount);
                    await _context.SaveChangesAsync();
                }
                else if (creditDebit.serviceId == 4)
                {
                    ExtraMethods.DebitUser(creditDebit.BrinqAccountNumber, creditDebit.Amount);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Name", transaction.PaymentMethodId);
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name");
                    ViewData["CreditsAndDebits"] = _context.Transaction.Where(t => t.ServiceId == 3 && t.ServiceId == 4).Include(t => t.PaymentMethod).Include(t => t.Service);
                    return View(creditDebit);
                }

                
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(BalanceUpdated), creditDebit);
            }
            //ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Name", transaction.PaymentMethodId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name");
            ViewData["CreditsAndDebits"] = _context.Transaction.Where(t => t.ServiceId == 3 && t.ServiceId == 4).Include(t => t.PaymentMethod).Include(t => t.Service);
            return View(creditDebit);
        }


        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Id");
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceId,PaymentMethodId,Amount,KaedcUser,Datetime,MeterName,Meternumber,TransactionReference,Hash,PayersName,ApiUniqueReference,GatewayresponseCode,GatewayresponseMessage,Statuscode,StatusMessage,RecipientPhoneNumber,Token,PhcnUnique,PayerIp,AgentProfit,CoordinatorProfit,BrinqProfit,TopUpValue")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Id", transaction.PaymentMethodId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Id", transaction.ServiceId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Id", transaction.PaymentMethodId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Id", transaction.ServiceId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceId,PaymentMethodId,Amount,KaedcUser,Datetime,MeterName,Meternumber,TransactionReference,Hash,PayersName,ApiUniqueReference,GatewayresponseCode,GatewayresponseMessage,Statuscode,StatusMessage,RecipientPhoneNumber,Token,PhcnUnique,PayerIp,AgentProfit,CoordinatorProfit,BrinqProfit,TopUpValue")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentMethodId"] = new SelectList(_context.Paymentmethod, "Id", "Id", transaction.PaymentMethodId);
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Id", transaction.ServiceId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.PaymentMethod)
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }
    }
}
