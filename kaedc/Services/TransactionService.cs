using kaedc.Interfaces;
using kaedc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Services
{
    public class TransactionService : ITransaction
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly Kaedc _context;

        public TransactionService(IHostingEnvironment hostingEnvironment, Kaedc context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public async Task<List<Transaction>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            var data = await GetData();
            return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetCount()
        {
            var data = await GetData();
            return data.Count;
        }

        private async Task<List<Transaction>> GetData()
        {
            //var json = await File.ReadAllTextAsync(Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "paging.json"));
            //return JsonConvert.DeserializeObject<List<Transaction]>>(json);
            var kaedc = _context.Transaction.Include(t => t.PaymentMethod).Include(t => t.Service).OrderByDescending(i => i.Datetime);
            return await kaedc.ToListAsync();
        }

    }
}
