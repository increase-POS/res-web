using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using posWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace resWebApp.Models
{
    public class DashBoardModel
    {
        #region Attributes
        public int branchId { get; set; }
        public int salesCount { get; set; }
        public int tablesCount { get; set; }
        public int diningHallCount { get; set; }
        public int takeawayCount { get; set; }
        public int selfServiceCount { get; set; }
        public int reservationsCount { get; set; }
        public int onLineUsersCount { get; set; }

        public decimal balance { get; set; }

        #endregion

        public async Task<DashBoardModel> GetDashBoardInfo(int? branchId,  int userId)
        {
            DashBoardModel items = new DashBoardModel();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());

            // 
            IEnumerable<Claim> claims = await APIResult.getList("WebDashBoard/GetDashBoardInfo", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items = JsonConvert.DeserializeObject<DashBoardModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return items;
        }

        public async Task getBasicSettings()
        {
            Global.basicSettings = new BasicSettings();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            string method = "WebDashBoard/getBasicSettings";
            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    Global.basicSettings = JsonConvert.DeserializeObject<BasicSettings>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
        }
        
    }
}