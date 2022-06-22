using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace resWebApp.Models
{
    public class PermissionModel
    {
        public bool showDashBoard { get; set; }
 
        public bool showSales { get; set; }
        public bool showReservations { get; set; }
        public bool showKitchen { get; set; }
        public bool showDelivery { get; set; }


        #region methods
        public async Task<PermissionModel> GetPermissions(int userId)
        {
            PermissionModel items = new PermissionModel();

            if (userId == 2)
            {
                items = new PermissionModel()
                {
                    showSales = true,
                    showDashBoard = true,
                    showKitchen =true,
                    showDelivery = true,
                    showReservations = true
                };
            }
            else
            {

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("userId", userId.ToString());
                // 
                IEnumerable<Claim> claims = await APIResult.getList("WebDashBoard/GetPermissions", parameters);

                foreach (Claim c in claims)
                {
                    if (c.Type == "scopes")
                    {
                        items = JsonConvert.DeserializeObject<PermissionModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                }
            }
            return items;
        }
        #endregion
    }
}