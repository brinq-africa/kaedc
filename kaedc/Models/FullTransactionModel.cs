using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class FullTransactionModel
    {
        public List<Transaction> transaction { get; set; }
        public AllTransactionPaginationModel pageModel { get; set; }
    }
}