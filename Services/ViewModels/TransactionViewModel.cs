using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class TransactionViewModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // Deposit, Withdraw, Transfer
        public DateTime Date { get; set; }
    }
}
