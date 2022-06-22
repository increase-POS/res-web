using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {
        BranchModel branchModel = new BranchModel();

        public async Task<ActionResult> Index(DashBoardModel dashBoardModel)
        {
            try { 
            #region set session values
            if (Request.Cookies["Cookie1"] != null)
            {
                Session["UserName"] = HttpUtility.UrlDecode(Request.Cookies["Cookie1"].Values["UserName"]);
                Session["UserID"] = Request.Cookies["Cookie1"].Values["UserID"];
                Session["Image"] = Request.Cookies["Cookie1"].Values["Image"];
                Session["lang"] = Request.Cookies["Cookie1"].Values["lang"];
                Session["isAdmin"] = Request.Cookies["Cookie1"].Values["isAdmin"];


                #region basic settings
                await dashBoardModel.getBasicSettings();
                Session["typesOfService_diningHall"] = Global.basicSettings.typesOfService_diningHall;
                Session["typesOfService_selfService"] = Global.basicSettings.typesOfService_selfService;
                Session["typesOfService_takeAway"] = Global.basicSettings.typesOfService_takeAway;
                    #endregion

                    #region get user permissions
                    PermissionModel permissionModel = new PermissionModel();
                    permissionModel = await permissionModel.GetPermissions(int.Parse(Session["UserID"].ToString()));
                    Session["showDashBoard"] = permissionModel.showDashBoard;
                    Session["showSales"] = permissionModel.showSales;
                    Session["showReservations"] = permissionModel.showReservations;
                    Session["showKitchen"] = permissionModel.showKitchen;
                    Session["showDelivery"] = permissionModel.showDelivery;
                    #endregion


                    if (bool.Parse(Session["showDashBoard"].ToString()) == false)
                    {
                        AccountController ac = new AccountController();
                        ac.ControllerContext = new ControllerContext(this.Request.RequestContext, ac);

                        return ac.RedirectUser();
                    }
                }
                #endregion

                #region get user image
                UserModel user = new UserModel();
            var image = Session["Image"];
            
            if ((Session["Image"].ToString() != "" && Session["info.image"] == null) || (Session["info.image"] != null && Session["info.image"].ToString() == "") )
            {
                if (image != null && image.ToString() != "")
                {
                    var imageArr = await user.downloadImage(Session["Image"].ToString());
                    Session["info.image"] = imageArr;//storing session.
                                                     //ViewBag.Image = imageArr;
                }
                else
                {
                    Session["info.image"] = "";
                }
            }
            #endregion
            #region get branches
            List<BranchModel> branches = new List<BranchModel>();
            if (int.Parse(Session["UserID"].ToString()) == 2)
                branches = await branchModel.GetAll("all");
            else
                branches = await branchModel.GetBranchesActive(int.Parse(Session["UserID"].ToString()));
            ViewBag.branches = branches;
            Session["branches"] = branches;
            #endregion

            dashBoardModel = await dashBoardModel.GetDashBoardInfo(dashBoardModel.branchId,  int.Parse(Session["UserID"].ToString()));
            
            
            return View(dashBoardModel);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RefreshDashBoard(int branchId)
        {
            DashBoardModel dashBoardModel = new DashBoardModel();

            dashBoardModel = await dashBoardModel.GetDashBoardInfo(branchId,  int.Parse(Session["UserID"].ToString()) );

            JsonResult result = this.Json(new
            {
                salesCount = dashBoardModel.salesCount,
                tablesCount = dashBoardModel.tablesCount,
                dinningHallCount = dashBoardModel.diningHallCount,
                selfServiceCount = dashBoardModel.selfServiceCount,
                takeawayCount = dashBoardModel.takeawayCount,
                reservationsCount = dashBoardModel.reservationsCount,
                onLineUsersCount = dashBoardModel.onLineUsersCount,
                balance = dashBoardModel.balance,
            }, JsonRequestBehavior.AllowGet);

            return result;
        }

        

        [AllowAnonymous]
        public ActionResult About()
        {
            try { 
            ViewBag.Message = "Your application description page.";

            return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            try
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            if (Session["lang"] == null)
                Session["lang"] = "en";
            return View();
        }
    }
}