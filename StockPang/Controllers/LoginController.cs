using StockPang.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StockPang.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return Redirect("/login/login");
        }

        [HttpGet]
        public ActionResult Login(string msg)
        {
            
            ViewData["msg"] = msg;



            return View();
        }

        public ActionResult LogOut(string msg)
        {
            //HttpCookie cookie = new HttpCookie("USER_ID");
            Response.Cookies["USER_ID"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["USER_INFO"].Expires = DateTime.Now.AddDays(-1);
            return Redirect("/Home/Index");
        }

        
        [HttpPost]
        public async Task<ActionResult> Login(string user_id, string user_pw)
        {
            try
            {
                bool data;
                DataSet data1;

                data = modelLogin.GetCheckUser(user_id, user_pw);

                if (data == false)
                {
                    return Redirect("/login/login?msg=로그인실패");
                }
                else
                {

                    data1 = modelLogin.GetUser(user_id);

                    string USER_NAME = data1.Tables[0].Rows[0]["USER_NAME"].ToString();
                    string USER_ALIAS = data1.Tables[0].Rows[0]["USER_ALIAS"].ToString();
                    string USER_STATUS = data1.Tables[0].Rows[0]["USER_STATUS"].ToString();


                    HttpCookie cookie = new HttpCookie("USER_INFO");
                    
                    cookie.Values["USER_ID"] =  Server.UrlEncode(Convert.ToString(user_id));
                    cookie.Values["USER_NAME"] = Server.UrlEncode(Convert.ToString(USER_NAME));
                    cookie.Values["USER_ALIAS"] = Server.UrlEncode(Convert.ToString(USER_ALIAS));
                    cookie.Values["USER_STATUS"] = Server.UrlEncode(Convert.ToString(USER_STATUS));

                    Response.SetCookie(cookie); //SetCookie() is used for update the cookie.
                    Response.Cookies.Add(cookie); //The Cookie.Add() used for Add the cookie.

                    return Redirect("/Home/Index");
                }
               

            }
            catch (Exception ex)
            {
                //실패
                return Redirect("/login/login?msg=로그인실패");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}
