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
    public class OrderPreparingModel
    {
        #region Attributes
        public int orderPreparingId { get; set; }
        public string orderNum { get; set; }
        public Nullable<int> invoiceId { get; set; }
        public string notes { get; set; }
        public Nullable<decimal> preparingTime { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        public string itemName { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public int quantity { get; set; }

        //order
        public string status { get; set; }
        public int num { get; set; }
        public decimal remainingTime { get; set; }
        public string waiter { get; set; }
        public Nullable<System.DateTime> preparingStatusDate { get; set; }

        public string tables { get; set; }
        //invoice
        public string invNum { get; set; }
        public string invType { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }

        public List<ItemOrderPreparing> items { get; set; }

        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        public Nullable<System.TimeSpan> invTime { get; set; }
        #endregion

        #region Methods
        public async Task<int> EditInvoiceOrdersStatus(int invoiceId,  OrderPreparingStatus statusObject)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "webDashBoard/EditInvoiceOrdersStatus";

            parameters.Add("invoiceId", invoiceId.ToString());

            string myContent = JsonConvert.SerializeObject(statusObject);
            parameters.Add("statusObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<int> EditOrderStatus(int orderId,  OrderPreparingStatus statusObject)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "webDashBoard/EditOrderStatus";

            parameters.Add("orderId", orderId.ToString());

            string myContent = JsonConvert.SerializeObject(statusObject);
            parameters.Add("statusObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<List<OrderPreparingModel>> GetKitchenPreparingOrders(int branchId, string status, int duration = 0)
        {
            List<OrderPreparingModel> items = new List<OrderPreparingModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            // status like "Listed, Cooking"
            parameters.Add("status", status);
            parameters.Add("duration", duration.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("OrderPreparing/GetKitchenPreparingOrders", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<OrderPreparingModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<OrderPreparingModel> GetPreparingOrderDetails(int orderId)
        {
            OrderPreparingModel items = new OrderPreparingModel();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("orderId", orderId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("webDashBoard/GetPreparingOrderDetails", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items=JsonConvert.DeserializeObject<OrderPreparingModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return items;
        }

        public async Task<int> EditPreparingOrdersPrepTime(int orderId,decimal preparingTime, string notes,int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "webDashBoard/EditPreparingOrdersPrepTime";

            parameters.Add("orderId", orderId.ToString());
            parameters.Add("notes", notes);

            parameters.Add("preparingTime", preparingTime.ToString());
            parameters.Add("userId", userId.ToString());
            return await APIResult.post(method, parameters);
        }
        #endregion
    }
    public class ItemOrderPreparing
    {
        #region Attributes
        public int itemOrderId { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> orderPreparingId { get; set; }
        public Nullable<int> quantity { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        public Nullable<int> itemId { get; set; }
        public string itemName { get; set; }
        public Nullable<int> categoryId { get; set; }
        public string categoryName { get; set; }
        public int sequence { get; set; }
        public List<itemsTransferIngredientsModel> itemsIngredients { get; set; }
        public List<ItemTransfer> itemExtras { get; set; }
        #endregion
    }
    public class itemsTransferIngredientsModel
    {
        public long itemsTransIngredId { get; set; }
        public Nullable<long> itemsTransId { get; set; }
        public Nullable<long> dishIngredId { get; set; }
        public Nullable<long> itemUnitId { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public string itemName { get; set; }
        public string DishIngredientName { get; set; }
        public bool isBasic { get; set; }
    }
    public class OrderPreparingStatus
    {
        #region Attributes
        public int orderStatusId { get; set; }
        public Nullable<int> orderPreparingId { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        #endregion
    }
}