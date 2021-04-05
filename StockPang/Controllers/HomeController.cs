using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;

namespace StockPang.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public class Stock_Money
        {
            public string value { get; set; }
        }

        public ActionResult Index()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string html = wc.DownloadString("https://finance.naver.com/item/main.nhn?code=005930");
            // oRTK 부터 ~ noHTP 사이의 Json만 적출해온다.
            string json = JsonExport.Between(html, "<p class=\"no_today\">", "</p>");
            json = JsonExport.Between(json, "<span class=\"blind\">", "</span>");

            var p = new Stock_Money { value = json };
            string jsonString = JsonConvert.SerializeObject(p);
            Stock_Money pObj = JsonConvert.DeserializeObject<Stock_Money>(jsonString);

            ViewBag.Value = json;

            return View();
        }

        public ActionResult GetValue()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string html = wc.DownloadString("https://finance.naver.com/item/main.nhn?code=005930");
            // oRTK 부터 ~ noHTP 사이의 Json만 적출해온다.
            string json = JsonExport.Between(html, "<p class=\"no_today\">", "</p>");
            json = JsonExport.Between(json, "<span class=\"blind\">", "</span>");

            //var p = new Stock_Money { value = json };
            //string jsonString = JsonConvert.SerializeObject(p);
            //Stock_Money pObj = JsonConvert.DeserializeObject<Stock_Money>(jsonString);

            var jsonData = json;
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockList()
        {
            return View();
        }
    }

    public static class JsonExport
    {
        public static string Between(this string source, string left, string right)
        {
            int from = source.IndexOf(left);
            if (from == -1) return "";
            from += left.Length;
            int to = source.IndexOf(right, from);
            if (to == -1) return "";
            String result = source.Substring(from, to - from);
            return result;
        }
    }
}
