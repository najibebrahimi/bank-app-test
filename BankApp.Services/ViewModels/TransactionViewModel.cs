using BankApp.Services.Enums;
using BankApp.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services.ViewModels
{
    public class TransactionViewModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        //public string Type { get; set; } // Deposit, Withdraw, Transfer
        public TransactionType Type { get; set; }
        public string TypeFormatted => Common.TransactionTypeToString(Type);
        public DateTime Date { get; set; }
    }
}
