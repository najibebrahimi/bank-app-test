using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class TransferViewModel
    {
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}
