using resWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace resWebApp.Models
{
    public class OrderPreparingViewModel
    {
        public IEnumerable<OrderPreparingModel> Orders { get; set; }
        public int OrderPerPage { get; set; } = Global.rowsInPage;
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Orders.Count() / (double)OrderPerPage));
        }
        public IEnumerable<OrderPreparingModel> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * OrderPerPage;
            return Orders.Skip(start).Take(OrderPerPage);
        }
    }
}