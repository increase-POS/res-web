using posWebApp.Models;
using resWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace resWebApp.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {
        // GET: Delivery
        public async Task<ActionResult> DeliveryList( int page = 1)
        {
            try { 
            #region delivery man Invoices
            InvoiceModel invoiceModel = new InvoiceModel();

            string invoiceStatus = "Ready,Collected,InTheWay,Done";
            var invoices = await invoiceModel.GetOrdersWithDelivery(invoiceStatus,int.Parse(Session["UserID"].ToString()));

            invoices = invoices.OrderBy(x => x.sequence).ToList();
            var invoicesView = new InvoiceViewModel
            {
                Invoices = invoices,
                CurrentPage = page
            };

            #endregion
            return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> InvoiceDetails(int invoiceId,string status)
        {
            try { 
            InvoiceModel invoiceModel = new InvoiceModel();
            #region Invoice Details
            ItemTransfer itemTransfer = new ItemTransfer();

            invoiceModel = await invoiceModel.GetByInvoiceId(invoiceId);
            invoiceModel.InvoiceItems = await itemTransfer.GetInvoicesItems(invoiceModel.invoiceId);
            invoiceModel.status = status;
            #endregion

            #region customer info
            AgentModel agentModel = new AgentModel();
            agentModel = await agentModel.getAgentById((int)invoiceModel.agentId);
            invoiceModel.Customer = agentModel;


            var image = agentModel.image;
            if (image != null && image.ToString() != "")
            {
                var imageArr = await agentModel.downloadImage(agentModel.image);
                ViewBag.Image = imageArr;
            }
            #endregion

            
            return View(invoiceModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> Confirm(int invoiceId)
        {
            try { 
            OrderPreparingStatus st = new OrderPreparingStatus();
            OrderPreparingModel preparingOrder = new OrderPreparingModel();


            st.status = "Collected";
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;

            int res = await preparingOrder.EditInvoiceOrdersStatus(invoiceId, st);


            return RedirectToAction("DeliveryList", "Delivery");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> changeDeliveryOrderStatus(int invoiceId, string status)
        {
            string nextStatus = "";

            #region status object

            OrderPreparingStatus st = new OrderPreparingStatus();
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;
            switch (status)
            {
                case "Ready":
                    st.status = "Collected";
                    nextStatus = "InTheWay";
                    break;
                case "Collected":
                    st.status = "InTheWay";
                    nextStatus = "Done";
                    break;
                case "InTheWay":
                    st.status = "Done";
                    break;
            }
            #endregion
            OrderPreparingModel preparingOrder = new OrderPreparingModel();

            int res = await preparingOrder.EditInvoiceOrdersStatus(invoiceId, st);            

            JsonResult result = this.Json(new
            {
                message = st.status,
                nextStatus = nextStatus,

            }, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}