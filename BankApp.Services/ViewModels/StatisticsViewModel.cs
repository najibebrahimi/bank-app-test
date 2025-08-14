using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Services.ViewModels
{
    public class StatisticsViewModel
    {
        public int CustomerCount { get; set; }
        public int AccountCount { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
