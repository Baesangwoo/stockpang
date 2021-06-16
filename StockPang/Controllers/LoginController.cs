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

        [HttpGet]
        public ActionResult Register(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }
        

        public ActionResult IDCheck( string User_Id )
        {


            bool data;
 
            data = modelLogin.GetIDCheck(User_Id);

            return Json(data, JsonRequestBehavior.AllowGet);


        }
        public ActionResult AliasCheck(string User_Alias)
        {


            bool data;

            data = modelLogin.GetAliasCheck(User_Alias);

            return Json(data, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult Register(string reg_user_id, string reg_user_id_chk, string reg_user_pw, string reg_user_pw2, string user_nickname,
                                        string user_nickname_chk, string user_phone, string user_email)
        {

            //빈 데이타 체크 
            if (string.IsNullOrEmpty(reg_user_id) || string.IsNullOrEmpty(reg_user_pw) || string.IsNullOrEmpty(user_nickname ))
            {
                return Redirect("/login/Register?msg=미입력 항목이 있습니다");
            }
            

            // 아이디 중복 체크 
            if (reg_user_id != reg_user_id_chk) {
                return Redirect("/login/Register?msg=아이디 중복 확인");
            } 

            // 패스워드 확인
            if (reg_user_pw != reg_user_pw2)
            {
                return Redirect("/login/Register?msg=패스워드 확인");
            } 

            // 닉네임 중복 체크 
            if (user_nickname != user_nickname_chk)
            {
                return Redirect("/login/Register?msg=닉네임 중복 확인");
            } 


            // 사용자 저장 
            modelLogin.SetUser(reg_user_id, reg_user_pw, user_nickname, user_phone, user_email);

            return Redirect("/login/login");


        }

    }


}
