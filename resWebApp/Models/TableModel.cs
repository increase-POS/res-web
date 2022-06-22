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
    public class TableModel
    {
        public int tableId { get; set; }
        public string name { get; set; }

        #region Methods
        public async Task<int> checkOpenedTable(int tableId, int branchId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/checkOpenedTable";
            parameters.Add("tableId", tableId.ToString());
            parameters.Add("branchId", branchId.ToString());
            return await APIResult.post(method, parameters);
        }

        public async Task<List<TableModel>> getReservationTables(int reservationId)
        {
            List<TableModel> items = new List<TableModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("reservationId", reservationId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getReservationTables", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<TableModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}