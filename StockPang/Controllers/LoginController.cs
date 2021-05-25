using System;
using System.Collections.Generic;
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
            return Redirect("/Home/Index");
        }

        
        [HttpPost]
        public async Task<ActionResult> Login(string user_id, string user_pw)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("USER_ID");
                cookie.Values["USER_ID"] = Convert.ToString(user_id);
                Response.SetCookie(cookie); //SetCookie() is used for update the cookie.
                Response.Cookies.Add(cookie); //The Cookie.Add() used for Add the cookie.
                
                return Redirect("/Home/Index");
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
