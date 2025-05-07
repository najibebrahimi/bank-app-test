using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProjectApi.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly BankAppDataContext _context;

        public StatisticsController(BankAppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var statistics = new Statistics
            {
                CustomerCount = await _context.Customers.CountAsync(),
                AccountCount = await _context.Accounts.CountAsync(),
                TotalBalance = await _context.Accounts.SumAsync(a => a.Balance)
            };

            return Ok(statistics);
        }
    }
}
