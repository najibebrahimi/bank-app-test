
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Utilities
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PagedResult(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }
    }
}



















    

















