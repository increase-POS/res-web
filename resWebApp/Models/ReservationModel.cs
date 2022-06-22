using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using resWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace resWebApp.Models
{
    public class ReservationModel
    {

        #region Attreibutes
        public long reservationId { get; set; }
        public string code { get; set; }
        public Nullable<int> customerId { get; set; }
        public string customerName { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<System.DateTime> reservationDate { get; set; }
        public Nullable<System.DateTime> reservationTime { get; set; }
        public Nullable<int> personsCount { get; set; }
        public string status { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }


        public string isExceed { get; set; }
        public IEnumerable<TableModel> tables { get; set; }
        public AgentModel customer { get; set; }

        public int sequence { get; set; }
        #endregion

        #region Methods

        public async Task<List<ReservationModel>> getDayReservtions(int userId, int branchId)
        {
            List<ReservationModel> items = new List<ReservationModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());
            parameters.Add("branchId", branchId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getDayReservtions", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ReservationModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<ReservationModel> getReservtionById(int reservationId)
        {
            ReservationModel item = new ReservationModel();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("reservationId", reservationId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getReservtionById", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item=JsonConvert.DeserializeObject<ReservationModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }

            AgentModel customer = new AgentModel();
            if(item.customerId != null)
                item.customer =await customer.getAgentById((int)item.customerId);

            
            return item;
        }

        internal async Task<int> confirmReservation(long reservationId, int userId,InvoiceModel invoice)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "webDashBoard/confirmReservation";
            parameters.Add("reservationId", reservationId.ToString());
            parameters.Add("userId", userId.ToString());

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("invoiceObject", myContent);

            return await APIResult.post(method, parameters);
        }

       
        #endregion
    }
}