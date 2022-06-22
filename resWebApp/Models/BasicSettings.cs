using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace posWebApp.Models
{
    public  class BasicSettings
    {
        public string accuracy { get; set; }
        public  string currency { get; set; }

        // invoice types in resturant
        public  string typesOfService_diningHall { get; set; }
        public  string typesOfService_takeAway { get; set; }
        public  string typesOfService_selfService { get; set; }
    }
}