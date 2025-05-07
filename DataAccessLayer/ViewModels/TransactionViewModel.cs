using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class TransactionViewModel
    {
        


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Kontonummer måste vara större än noll")]
        public int AccountId { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } // Deposit, Withdraw, Transfer

        public DateTime Date { get; set; }
    }
}
