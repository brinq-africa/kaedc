using kaedc.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class AllTransactionPaginationModel : PageModel
    {
        private readonly ITransaction _transaction;

        public AllTransactionPaginationModel(ITransaction transaction)
        {
            _transaction = transaction;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public List<Transaction> Data { get; set; }
    }
}

