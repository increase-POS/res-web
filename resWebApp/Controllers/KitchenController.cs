using posWebApp.Models;
using resWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace posWebApp.Controllers
{
    public class KitchenController : Controller
    {
        // GET: Kitchen
        public async Task<ActionResult> OrderPreparingList(int branchId,int page = 1)
        {
            try
            {
                #region get branches
                List<BranchModel> branches;
                if (Session["branches"] != null)
                    branches = (List<BranchModel>)Session["branches"];
                else
                {
                    BranchModel branchModel = new BranchModel();
                    branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
                    Session["branches"] = branches;
                }

                if (branches.Count > 0 && branchId == 0)
                    branchId = branches[0].branchId;

                Session["branchId"] = branchId;
                ViewBag.branches = new SelectList(branches, "branchId", "name", branchId);
                #endregion

                #region preparing orders
                OrderPreparingModel preparingOrder = new OrderPreparingModel();
                List<string> statusLst = new List<string>() { "Listed", "Preparing", "Ready" };

                int duration = 24;


                var orders = await preparingOrder.GetKitchenPreparingOrders(branchId, "", duration);
                orders = orders.Where(x => statusLst.Contains(x.status)).ToList();
                orders = orders.Where(x => x.status != "Ready" || (x.status == "Ready" && x.shippingCompanyId == null)).ToList();


                var orderView = new OrderPreparingViewModel
                {
                    Orders = orders,
                    CurrentPage = page
                };
                #endregion
                return View(orderView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> OrderPreparingDetails(int orderId)
        {
            try { 
            OrderPreparingModel orderPreparing = new OrderPreparingModel();
            var order = await orderPreparing.GetPreparingOrderDetails(orderId);
            return View(order);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SavePreparingTime(int orderId,decimal time, string notes)
        {
            
            OrderPreparingModel preparingOrder = new OrderPreparingModel();

            int res = await preparingOrder.EditPreparingOrdersPrepTime(orderId, time,notes, int.Parse(Session["UserID"].ToString()));

            JsonResult result = this.Json(new
            {
                message = "",

            }, JsonRequestBehavior.AllowGet);

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> changeOrderStatus(int orderId, string status)
        {
            string nextStatus = "";

            #region status object

            OrderPreparingStatus st = new OrderPreparingStatus();
            st.createUserId = int.Parse(Session["UserID"].ToString());
            st.isActive = 1;
            switch (status)
            {
                case "Listed":
                    st.status = "Preparing";
                    nextStatus = "Ready";
                    break;
                case "Preparing":
                    st.status = "Ready";
                    nextStatus = "Done";
                    break;
                case "Ready":
                    st.status = "Done";
                    break;
            }
            #endregion
            OrderPreparingModel preparingOrder = new OrderPreparingModel();

            int res = await preparingOrder.EditOrderStatus(orderId, st);

            JsonResult result = this.Json(new
            {
                message = st.status,
                nextStatus = nextStatus,

            }, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}