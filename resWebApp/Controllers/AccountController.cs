﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using resWebApp.Models;
using System.Web.Security;
using resWebApp.Resources;
//using System.Web.Http;

namespace resWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
       [AllowAnonymous]
        public async Task<ActionResult> Login(UserModel userModel)
        {
            try
            {
                DashBoardModel dashBoardModel = new DashBoardModel();

                #region get user from cookie
                if (Request.Cookies["Cookie1"] != null)
                {
                    string name = Request.Cookies["Cookie1"].Values["Image"];
                    Session["UserName"] = Request.Cookies["Cookie1"].Values["UserName"];
                    Session["UserID"] = Request.Cookies["Cookie1"].Values["UserID"];
                    Session["Image"] = Request.Cookies["Cookie1"].Values["Image"];
                    Session["lang"] = Request.Cookies["Cookie1"].Values["lang"];
                    Session["isAdmin"] = Request.Cookies["Cookie1"].Values["isAdmin"];


                    #region get user permissions
                    PermissionModel permissionModel = new PermissionModel();
                    permissionModel = await permissionModel.GetPermissions(int.Parse(Session["UserID"].ToString()));
                    Session["showDashBoard"] = permissionModel.showDashBoard;
                    Session["showSales"] = permissionModel.showSales;
                    Session["showReservations"] = permissionModel.showReservations;
                    Session["showKitchen"] = permissionModel.showKitchen;
                    Session["showDelivery"] = permissionModel.showDelivery;
                    #endregion


                    #region redirect according to permission

                    return RedirectUser();

                    #endregion
                    
                }
                #endregion
                #region first load
                if (userModel.username == "" || userModel.username == null)
                {
                    ViewBag.message = "";
                    return View(userModel);
                }
                #endregion
                #region after post
                string password = HelpClass.MD5Hash("Inc-m" + userModel.password);
                var user = await userModel.Getloginuser(userModel.username, password);
                if (user.userId == 0)
                {
                    ViewBag.message = AppResource.InvalidLogin;
                    return View(userModel);
                }
                else
                {
                    #region get user lang
                    var lang = await userModel.getUserLanguage(user.userId);
                    #endregion

                    #region get user permissions
                    PermissionModel permissionModel = new PermissionModel();
                    permissionModel = await permissionModel.GetPermissions(user.userId);

                    if (userModel.userId == 2)
                        userModel.isAdmin = true;
                    else
                        userModel.isAdmin = false;
                    #endregion

                    bool rememberMe = false;
                    if (userModel.RememberMe)
                        rememberMe = true;

                    userModel = user;
                    #region remember me
                    if (rememberMe)
                    {
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                   (
                                   1,
                                   userModel.username,
                                   DateTime.Now,
                                   DateTime.Now.AddMinutes(60), // expiry
                                   rememberMe,
                                   "",
                                   "/"
                                   );

                        //encrypt the ticket and add it to a cookie
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie("Cookie1", enTicket);
                        cookie.Expires = DateTime.Now.AddMinutes(60);
                        cookie.HttpOnly = false;
                        cookie.Values.Add("UserName", HttpUtility.UrlEncode(userModel.fullName));
                        cookie.Values.Add("UserId", userModel.userId.ToString());
                        cookie.Values.Add("Image", userModel.image);
                        cookie.Values.Add("lang", lang);
                        cookie.Values.Add("isAdmin", userModel.isAdmin.ToString());

                        Response.Charset = "UTF-8";
                        Response.Cookies.Add(cookie);



                    }
                    else
                    {
                        //give user authintication
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                                  (
                                  1,
                                  userModel.username,
                                  DateTime.Now,
                                  DateTime.Now.AddMinutes(5), // expiry
                                  false,
                                  "",
                                  "/"
                                  );

                        //encrypt the ticket and add it to a cookie
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie("Cookie1", enTicket);
                        cookie.Expires = DateTime.Now.AddMinutes(5);
                        cookie.HttpOnly = false;
                        cookie.Values.Add("UserName", HttpUtility.UrlEncode(userModel.fullName));
                        cookie.Values.Add("UserId", userModel.userId.ToString());
                        cookie.Values.Add("Image", userModel.image);
                        cookie.Values.Add("lang", lang);
                        cookie.Values.Add("isAdmin", userModel.isAdmin.ToString());

                        Response.Charset = "UTF-8";
                        Response.Cookies.Add(cookie);
                    }

                    FormsAuthentication.SetAuthCookie(userModel.username, false);
                    #endregion


                    Session["UserName"] = userModel.fullName;
                    Session["UserID"] = userModel.userId;
                    Session["Image"] = userModel.image;
                    Session["info.image"] = "";
                    Session["lang"] = lang;
                    Session["isAdmin"] = userModel.isAdmin.ToString();

                    Session["showDashBoard"] = permissionModel.showDashBoard;
                    Session["showSales"] = permissionModel.showSales;
                    Session["showReservations"] = permissionModel.showReservations;
                    Session["showKitchen"] = permissionModel.showKitchen;
                    Session["showDelivery"] = permissionModel.showDelivery;

                    #region redirect

                    return RedirectUser();

                    #endregion
                }
                #endregion
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult RedirectUser()
        {
            if(Session["showDashBoard"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (bool.Parse(Session["showDashBoard"].ToString()) == true)
            {
                return RedirectToAction("Index", "Home", new { redirect = 1 });
            }
            else if (bool.Parse(Session["showSales"].ToString()) == true)
            {
                return RedirectToAction("sales", "Sales");
            }
            else if (bool.Parse(Session["showReservations"].ToString()) == true)
            {
                return RedirectToAction("Reservations", "Reservations");
            }
            else if (bool.Parse(Session["showKitchen"].ToString()) == true)
            {
                return RedirectToAction("OrderPreparingList", "Kitchen");
            }
            else if (bool.Parse(Session["showDelivery"].ToString()) == true)
            {
                return RedirectToAction("DeliveryList", "Delivery");
            }         
            else
            {
                return RedirectToAction("About", "Home");

            }
        }
        public ActionResult Logout()
        {
            try
            {
                //clear cookie
                if (Request.Cookies["Cookie1"] != null)
                {
                    var c = new HttpCookie("Cookie1");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }

                // remove authintication
                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            catch {
                return RedirectToAction("Error", "Home");
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> removeAuthentication(int agentId)
        //{

        //    // remove authintication
        //    FormsAuthentication.SignOut();
        //    Session.Abandon();
           
        //    JsonResult result = this.Json(new
        //    {
        //        res = "sucssess"
        //    }, JsonRequestBehavior.AllowGet);

        //    return result;
        //}
    }
}