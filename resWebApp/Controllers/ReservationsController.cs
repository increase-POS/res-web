using resWebApp.Models;
using resWebApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace resWebApp.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: Reservation
        public async Task<ActionResult> Reservations(int branchId, int page = 1)
        {
            try { 
            Session["branchId"] = branchId;
            #region reservations
            ReservationModel resModel = new ReservationModel();
            var res = await resModel.getDayReservtions(int.Parse(Session["UserID"].ToString()),branchId);

            var resView = new ReservationViewModel
            {
                Reservtions = res,
                CurrentPage = page
            };

            #endregion

            #region get branches
            BranchModel branchModel = new BranchModel();
            List<BranchModel> branches;
            if (Session["branches"] != null)
                branches = (List<BranchModel>)Session["branches"];
            else
            {
                branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
                Session["branches"] = branches;
            }
            ViewBag.branches = new SelectList(branches, "branchId", "name", branchId);
            #endregion

            return View(resView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> ReservtionDetails(int reservationId)
        {
            try { 
            #region reservations
            ReservationModel resModel = new ReservationModel();
            var res = await resModel.getReservtionById(reservationId);

            #endregion

            #region customer image
            string imageArr = "";
            if (res.customer != null)
            {
                AgentModel agentModel = new AgentModel();
                imageArr = await agentModel.downloadImage(res.customer.image);
            }
            ViewBag.Image = imageArr;
            #endregion
            return View(res);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<ActionResult> ReservtionConfirm(int reservationId,int? customerId,int branchId)
        {
            try {

                    #region invoice object
                    InvoiceModel invoice = new InvoiceModel();

                    invoice.invNumber = await invoice.generateDialyInvNumber("s,sd,ssd,ss,tsd,ts", branchId);
                    invoice.invType = "sd";
                    invoice.agentId = customerId;
                    invoice.reservationId = reservationId;
                    invoice.branchCreatorId = branchId;
                    //invoice.posId = MainWindow.posLogin.posId;
                    invoice.branchId = branchId;
                    invoice.createUserId = int.Parse(Session["userId"].ToString());
                    invoice.discountValue = 0;
                    invoice.total = 0;
                    invoice.totalNet = 0;
                    invoice.paid = 0;
                    invoice.deserved = 0;
                    invoice.tax = 0;
                    invoice.taxtype = 0;
                    invoice.isApproved = 0;
                    #endregion
                    ReservationModel resModel = new ReservationModel();

                    var res = await resModel.confirmReservation(reservationId, int.Parse(Session["UserID"].ToString()), invoice);

                //}

                //return RedirectToAction("ReservtionDetails", "Reservations", new { reservationId = reservationId });

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

            JsonResult result = this.Json(new
            {
                message = "",

            }, JsonRequestBehavior.AllowGet);

            return result;
        }


        [HttpPost]
       public async Task<ActionResult> validateTablesBeforeConfirm(int branchId, int reservationId)
        {
            try { 
            string message = "";
            int openedTables = 0;
            TableModel tableModel = new TableModel();
            var tables = await tableModel.getReservationTables(reservationId);

            foreach (TableModel tb in tables)
            {
                int notOpened = await tableModel.checkOpenedTable(tb.tableId, branchId);

                if (notOpened == 0)
                {
                    openedTables++;
                    if (message == "")
                        message += tb.name;
                    else
                        message += ", " + tb.name;
                }

            }

            if (message != "")
            {
                if (openedTables > 1)
                    message += " " + AppResource.EmptyAre;
                else
                    message+= " " + AppResource.EmptyIs;

                message += " " + AppResource.Opened;
            }

            JsonResult result = this.Json(new
            {
                message = message,

            }, JsonRequestBehavior.AllowGet);

            return result;
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}