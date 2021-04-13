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
using StockPang.Models;
using System.Data;

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

        [HttpGet]
        public ActionResult StockList(string Serch_text, string Serch_psr, string Serch_por, string Serch_per, string Serch_biz_rate, string Serch_net_rate)
        {
            DataSet data = null;
            
            data = modelStockList.GetSearchData(Serch_text, Serch_psr, Serch_por, Serch_per, Serch_biz_rate, Serch_net_rate);
            
            data.Tables[0].Columns.Add("TOTAL_AMT_C");
            data.Tables[0].Columns.Add("SALES_AMT_C");
            data.Tables[0].Columns.Add("BIZ_PROFIT_C");
            data.Tables[0].Columns.Add("NET_PROFIT_C");

            data.Tables[0].Columns.Add("CAL_PSR");
            data.Tables[0].Columns.Add("CAL_POR");
            data.Tables[0].Columns.Add("CAL_PER");

            data.Tables[0].Columns.Add("BIZ_RATE");              //영업이익율 
            data.Tables[0].Columns.Add("NET_RATE");              //순이익율 



            foreach (DataRow row in data.Tables[0].Rows)
            {
                row["CAL_PSR"] = 0;
                row["CAL_POR"] = 0;
                row["CAL_PER"] = 0;

                row["BIZ_RATE"] = 0;
                row["NET_RATE"] = 0;


                string Data01 = row["TOTAL_AMT"].ToString();    //시총 
                string Data02 = row["SALES_AMT"].ToString();    //매출액 
                string Data03 = row["BIZ_PROFIT"].ToString();   //영업이익 
                string Data04 = row["NET_PROFIT"].ToString();   //순이익 

                //문자로 변경한 결과값을 보관하지 못해서 컬럼이 스트링형으로 만들어짐 
                row["TOTAL_AMT_C"] = string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data01)).TrimStart().Replace(" ", ",");
                row["SALES_AMT_C"] = string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data02)).TrimStart().Replace(" ", ",");


                if (Convert.ToDouble(row["BIZ_PROFIT"].ToString()) >= 0)
                {
                    row["BIZ_PROFIT_C"] = string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data03)).TrimStart().Replace(" ", ",");
                }
                else
                {
                    row["BIZ_PROFIT_C"] = "-" + string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data03) * -1).TrimStart().Replace(" ", ",");
                }

                if (Convert.ToDouble(row["NET_PROFIT"].ToString()) >= 0)
                {
                    row["NET_PROFIT_C"] = string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data04)).TrimStart().Replace(" ", ",");
                }
                else
                {
                    row["NET_PROFIT_C"] = "-" + string.Format("{0:# #### #### #### #### ####}", Convert.ToInt32(Data04) * -1).TrimStart().Replace(" ", ",");
                }


                //PSR : 주가매출배율
                if (Convert.ToInt32(row["SALES_AMT"].ToString()) != 0)
                {
                    row["CAL_PSR"] = (Convert.ToDouble(row["TOTAL_AMT"].ToString()) / Convert.ToDouble(row["SALES_AMT"].ToString())).ToString("0.00");
                }

                //POR : 주가영업이익배율
                if (Convert.ToInt32(row["BIZ_PROFIT"].ToString()) != 0)
                {
                    row["CAL_POR"] = (Convert.ToDouble(row["TOTAL_AMT"].ToString()) / Convert.ToDouble(row["BIZ_PROFIT"].ToString())).ToString("0.00");
                }
                //PER : 주가이익배율 
                if (Convert.ToInt32(row["NET_PROFIT"].ToString()) != 0)
                {
                    row["CAL_PER"] = (Convert.ToDouble(row["TOTAL_AMT"].ToString()) / Convert.ToDouble(row["NET_PROFIT"].ToString())).ToString("0.00");
                }

                //BIZ_RATE : 영업이익율 
                if (Convert.ToInt32(row["SALES_AMT"].ToString()) != 0)
                {
                    row["BIZ_RATE"] = (Convert.ToDouble(row["BIZ_PROFIT"].ToString()) / Convert.ToDouble(row["SALES_AMT"].ToString()) * 100).ToString("0.00");
                }
                //NET_RATE : 순이익율 
                if (Convert.ToInt32(row["SALES_AMT"].ToString()) != 0)
                {
                    row["NET_RATE"] = (Convert.ToDouble(row["NET_PROFIT"].ToString()) / Convert.ToDouble(row["SALES_AMT"].ToString()) * 100).ToString("0.00");
                }
            }

            return View(data);
        }


        public ActionResult StockDetail()
        {

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
