using kaedc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Interfaces
{
    public interface ITransaction
    {
        Task<List<Transaction>> GetPaginatedResult(int currentPage, int pageSize = 10);
        Task<int> GetCount();
    }
}
