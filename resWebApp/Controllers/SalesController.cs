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
    public class SalesController : Controller
    {
        // GET: Delivery
        public async Task<ActionResult> Sales()
        {
            try { 
            BranchModel branchModel = new BranchModel();
            List<BranchModel> branches;

            if(Session["branchId"]==null)
                Session["branchId"] = 0;


            if (Session["branches"] != null)
                branches = (List<BranchModel>)Session["branches"];
            else
                branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));

            ViewBag.branches = branches;
            return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        
        public async Task<ActionResult> SalesInvoices(string invoiceType, int branchId,int page = 1)
        {
            try { 
                Session["branchId"] = branchId;
                Session["invoiceType"] = invoiceType;
       

                #region sales Invoices
                InvoiceModel invoiceModel = new InvoiceModel();
                var invoices = await invoiceModel.getSalesInvoiceByType(int.Parse(Session["UserID"].ToString()), branchId, invoiceType);

                invoices = invoices.OrderBy(x => x.sequence).ToList();
                var invoicesView = new InvoiceViewModel
                {
                    Invoices = invoices,
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

                ViewBag.invoiceType = invoiceType;
                return View(invoicesView);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}