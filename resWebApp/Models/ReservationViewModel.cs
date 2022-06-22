using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace resWebApp.Models
{
    public class ReservationViewModel
    {
        public IEnumerable<ReservationModel> Reservtions { get; set; }
        public AgentModel Agent { get; set; }
        public int ReservPerPage { get; set; } = Global.rowsInPage;
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Reservtions.Count() / (double)ReservPerPage));
        }
        public IEnumerable<ReservationModel> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * ReservPerPage;
            return Reservtions.Skip(start).Take(ReservPerPage);
        }
    }
}