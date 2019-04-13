using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kaedc.Models;

namespace kaedc.Controllers
{
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
