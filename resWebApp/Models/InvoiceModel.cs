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
    public class InvoiceModel
    {

        #region Attributes

        public int invoiceId { get; set; }
        public Nullable<long> reservationId { get; set; }
        public string invNumber { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string invType { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> discountValue { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> invoiceMainId { get; set; }
        public string invCase { get; set; }
        public Nullable<System.TimeSpan> invTime { get; set; }
        public string notes { get; set; }
        public string vendorInvNum { get; set; }
        public string name { get; set; }
        public string branchName { get; set; }
        public Nullable<System.DateTime> vendorInvDate { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> itemsCount { get; set; }
        public Nullable<decimal> tax { get; set; }
        public decimal cashReturn { get; set; }
        public Nullable<int> taxtype { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<byte> isApproved { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public Nullable<int> shipUserId { get; set; }
        public string shipUserName { get; set; }
        public string shipCompanyName { get; set; }

        public string status { get; set; }
        public int invStatusId { get; set; }
        public decimal manualDiscountValue { get; set; }
        public string manualDiscountType { get; set; }
        public string createrUserName { get; set; }
        public decimal shippingCost { get; set; }
        public decimal realShippingCost { get; set; }
        public bool isActive { get; set; }
        public string payStatus { get; set; }
        public int sequence { get; set; }
        public string agentName { get; set; }


        public AgentModel Customer { get; set; }
        public List<ItemTransfer> InvoiceItems { get; set; }
        #endregion

        #region methods
        public async Task<List<InvoiceModel>> getSalesInvoiceByType( int userId,int branchId, string type)
        {
            List<InvoiceModel> items = new List<InvoiceModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("invType", type);

            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/getBranchInvoices", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<string> generateDialyInvNumber(string invType, int branchId)
        {
            int sequence = await GetLastDialyNumOfInv(invType, branchId);
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 9999)
                strSeq = sequence.ToString().PadLeft(4, '0');
            string invoiceNum = strSeq;
            return invoiceNum;
        }
        public async Task<int> GetLastDialyNumOfInv(string invType, int branchId)
        {
            int count = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invType", invType);
            parameters.Add("branchId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Invoices/GetLastDialyNumOfInv", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    count = int.Parse(c.Value);
                    break;
                }
            }
            return count;
        }

        public async Task<List<InvoiceModel>> GetOrdersWithDelivery( string status,int userId)
        {
            List<InvoiceModel> items = new List<InvoiceModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());
            // status in syntax "Listed, Collected" or one status "Collected"
            // status values:  Ready, Collected, InTheWay,Done (not paid)
            parameters.Add("status", status.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/GetOrdersWithDelivery", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


      
       
        public async Task<InvoiceModel> GetByInvoiceId(int itemId)
        {
            InvoiceModel item = new InvoiceModel();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Invoices/GetByInvoiceId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<InvoiceModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


        #endregion
    }

    
}