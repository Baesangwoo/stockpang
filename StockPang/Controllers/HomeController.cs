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

        // 전체  List 
        [HttpGet]
        public ActionResult StockList(string Search_name, string Search_class, string Search_psr, string Search_por, string Search_per, string Search_biz_rate, string Search_net_rate)
        {
            DataSet data = null;
            
            data = modelStockList.GetSearchData(Search_name, Search_class, Search_psr, Search_por, Search_per, Search_biz_rate, Search_net_rate, "All", "0");
            
            data.Tables[0].Columns.Add("TOTAL_AMT_C");
            data.Tables[0].Columns.Add("SALES_AMT_C");
            data.Tables[0].Columns.Add("BIZ_PROFIT_C");
            data.Tables[0].Columns.Add("NET_PROFIT_C");

            foreach (DataRow row in data.Tables[0].Rows)
            {

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

            }

            return View("StockList", data);
        }

        // User별 List 
        public ActionResult UserStock(string Search_name, string Search_class, string Search_psr, string Search_por, 
                        string Search_per, string Search_biz_rate, string Search_net_rate)
        {
            DataSet data = null;
            string userID;
            if (Request.Cookies["USER_INFO"] == null)
            {
                userID = "0";
            }
            else
            {
                userID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }
            
            data = modelStockList.GetSearchData(Search_name, Search_class, Search_psr, Search_por, Search_per, Search_biz_rate, Search_net_rate, "User", userID);

            data.Tables[0].Columns.Add("TOTAL_AMT_C");
            data.Tables[0].Columns.Add("SALES_AMT_C");
            data.Tables[0].Columns.Add("BIZ_PROFIT_C");
            data.Tables[0].Columns.Add("NET_PROFIT_C");

            foreach (DataRow row in data.Tables[0].Rows)
            {

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

            }

            return View("StockList", data);
        }

        public ActionResult StockList2( string Search_name, string Search_class, string Search_per, string Search_gap, string Search_sa_rate
                                    ,   string Search_bp_rate, string Search_np_rate
                                )
        {
            DataSet data = null;

            data = modelStockList.GetSearchData2(Search_name, Search_class, Search_per, Search_gap, Search_sa_rate, Search_bp_rate, Search_np_rate, "All", "0");

            data.Tables[0].Columns.Add("TOTAL_AMT_C");
            data.Tables[0].Columns.Add("SALES_AMT_C");
            data.Tables[0].Columns.Add("BIZ_PROFIT_C");
            data.Tables[0].Columns.Add("NET_PROFIT_C");

            data.Tables[0].Columns.Add("PRICE_GAP");
            data.Tables[0].Columns.Add("PRICE_GAP_RATE");

            foreach (DataRow row in data.Tables[0].Rows)
            {

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

                if (Convert.ToDouble(row["NAVER_PRICE"].ToString()) != 0) {
                    row["PRICE_GAP"] = Convert.ToDecimal(row["NAVER_PRICE"].ToString()) - Convert.ToDecimal(row["STOCK_PRICE"].ToString());
                    row["PRICE_GAP_RATE"] = Convert.ToDecimal(Convert.ToDecimal(row["PRICE_GAP"].ToString()) / Convert.ToDecimal(row["NAVER_PRICE"].ToString()) * 100).ToString("0.0#");
                }
                else {
                    row["PRICE_GAP"] = 0.ToString("0.0#");
                    row["PRICE_GAP_RATE"] = 0.ToString("0.0#");
                }
    
            }

            return View("StockList2", data);
        }

        public ActionResult UserStock2( string Search_name, string Search_class, string Search_per, string Search_gap, string Search_sa_rate
                                    ,   string Search_bp_rate, string Search_np_rate
                                )
        {
            DataSet data = null;
            string userID;
            if (Request.Cookies["USER_INFO"] == null)
            {
                userID = "0";
            }
            else
            {
                userID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }

            data = modelStockList.GetSearchData2(Search_name, Search_class, Search_per, Search_gap, Search_sa_rate, Search_bp_rate, Search_np_rate, "User", userID);

            data.Tables[0].Columns.Add("TOTAL_AMT_C");
            data.Tables[0].Columns.Add("SALES_AMT_C");
            data.Tables[0].Columns.Add("BIZ_PROFIT_C");
            data.Tables[0].Columns.Add("NET_PROFIT_C");

            data.Tables[0].Columns.Add("PRICE_GAP");
            data.Tables[0].Columns.Add("PRICE_GAP_RATE");

            foreach (DataRow row in data.Tables[0].Rows)
            {

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

                if (Convert.ToDouble(row["NAVER_PRICE"].ToString()) != 0)
                {
                    row["PRICE_GAP"] = Convert.ToDecimal(row["NAVER_PRICE"].ToString()) - Convert.ToDecimal(row["STOCK_PRICE"].ToString());
                    row["PRICE_GAP_RATE"] = Convert.ToDecimal(Convert.ToDecimal(row["PRICE_GAP"].ToString()) / Convert.ToDecimal(row["NAVER_PRICE"].ToString()) * 100).ToString("0.0#");
                }
                else
                {
                    row["PRICE_GAP"] = 0.ToString("0.0#");
                    row["PRICE_GAP_RATE"] = 0.ToString("0.0#");
                }

            }

            return View("StockList2", data);
        }

        public ActionResult UserStock3(string Search_name, string Search_class, string Search_buy_gap, string Search_sell_gap
                                )
        {
            DataSet data = null;
            string userID;
            if (Request.Cookies["USER_INFO"] == null)
            {
                userID = "0";
            }
            else
            {
                userID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }

            data = modelStockList.GetSearchData3(Search_name, Search_class, Search_buy_gap, Search_sell_gap, "User", userID);
            data.Tables[0].Columns.Add("BUY_GAP_C");
            data.Tables[0].Columns.Add("SELL_GAP_C"); 
            data.Tables[0].Columns.Add("AVG_GAP_C");

            data.Tables[0].Columns.Add("BUY_GAP_RATE_C");
            data.Tables[0].Columns.Add("SELL_GAP_RATE_C");
            data.Tables[0].Columns.Add("AVG_GAP_RATE_C");


            data.Tables[0].Columns.Add("BUY_AVG_C");
            data.Tables[0].Columns.Add("SELL_AVG_C");
            data.Tables[0].Columns.Add("AVG_AVG_C");

            data.Tables[0].Columns.Add("STOCK_SAVE");

            foreach (DataRow row in data.Tables[0].Rows)
            {
                double bValue = Convert.ToDouble(row["BUY_GAP"].ToString());
                double sValue = Convert.ToDouble(row["SELL_GAP"].ToString());
                double aValue = Convert.ToDouble(row["AVG_GAP"].ToString());

                double bAvg = Convert.ToDouble(row["BUY_AVG"].ToString());
                double sAvg = Convert.ToDouble(row["SELL_AVG"].ToString());
                double aAvg = Convert.ToDouble(row["AVG_AVG"].ToString());


                double bRate = Convert.ToDouble(row["BUY_GAP_RATE"].ToString());
                double sRate = Convert.ToDouble(row["SELL_GAP_RATE"].ToString());


                row["BUY_GAP_C"] = bValue.ToString("0.00");
                row["SELL_GAP_C"] = sValue.ToString("0.00");
                row["AVG_GAP_C"] = aValue.ToString("0.00");

                row["BUY_AVG_C"] = bAvg.ToString("0.00");
                row["SELL_AVG_C"] = sAvg.ToString("0.00");
                row["AVG_AVG_C"] = aAvg.ToString("0.00");

                row["BUY_GAP_RATE_C"] = bRate.ToString("0.00");
                row["SELL_GAP_RATE_C"] = sRate.ToString("0.00");


                row["STOCK_SAVE"] = "저장";

            }

            return View("UserStock3", data);
        }

        public ActionResult StockReg(string Search_name, string Search_class)
        {

            string User_ID;
            if (Request.Cookies["USER_INFO"] == null)
            {
                User_ID = "0";
            }
            else
            {
                User_ID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }

            if (User_ID == "0")
            {
                return Redirect("/login/login");
            }

            DataSet data = null;

            data = modelStockList.GetSearchReg(Search_name, Search_class );

            data.Tables[0].Columns.Add("STOCK_REG");
            data.Tables[0].Columns.Add("STOCK_DEL");

            foreach (DataRow row in data.Tables[0].Rows)
            {

                row["STOCK_REG"] = "등록";    //등록 
                row["STOCK_DEL"] = "삭제";    //삭제 

            }

            return View(data);
        }

        public ActionResult RemarkSave(string dtl_stock_code, string dtl_stock_remark, string ViewName)
        {
            DataSet data = null;

            modelStockList.SetRemarkSave(dtl_stock_code, dtl_stock_remark);

            return Redirect(ViewName);
        }

        public ActionResult Reg_Insert(string Reg_Code, string Reg_Name, string Reg_Class1, string Reg_Class2, string Reg_Remark)
        {

            string userID;
            bool Result;

            if (string.IsNullOrEmpty(Reg_Code))
            {
                return Redirect("StockReg");
            }


            if (Request.Cookies["USER_INFO"] == null)
            {
                userID = "0";
            }
            else
            {
                userID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }


            Result = modelStockList.SetStockInsert(Reg_Code, Reg_Name, Reg_Class1, Reg_Class2, Reg_Remark, userID);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Reg_Delete(string Reg_Code)
        {

            bool Result;
            string userID;

            if (Request.Cookies["USER_INFO"] == null)
            {
                userID = "0";
            }
            else
            {
                userID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }

            Result = modelStockList.SetRegDelete(Reg_Code, userID);


            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save_Price(string Stock_Code, string Buy_Price, string Sell_Price, string Avg_Price)
        {
            DataSet data = null;
            bool Result;
            string User_ID;
            if (Request.Cookies["USER_INFO"] == null)
            {
                User_ID = "0";
            }
            else
            {
                User_ID = Request.Cookies["USER_INFO"].Values["USER_ID"].ToString();
            }

            Result = modelStockList.SetSavePrice(Stock_Code, User_ID, Buy_Price, Sell_Price, Avg_Price);


            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockDelete(string Stock_Code)
        {
            DataSet data = null;

            modelStockList.SetStockDelete(Stock_Code);


            return Redirect("StockReg");
        }

        public class modelAvgKospi
        {
            public string AVG_INDEX;
            public string INDEX_DATE;

        }

        public ActionResult AvgKospi()
        {
            DataSet model = null;

            model = modelChart.AvgKospi();
            List<modelAvgKospi> List = new List<modelAvgKospi>();

            for (int iloopCount = 0; iloopCount < model.Tables[0].Rows.Count; iloopCount++)
            {
                modelAvgKospi data = new modelAvgKospi();
                data.AVG_INDEX = model.Tables[0].Rows[iloopCount]["AVG_POINT"].ToString();

                data.INDEX_DATE = model.Tables[0].Rows[iloopCount]["INDEX_DATE"].ToString();

                List.Add(data);
            }

            return Json(List, JsonRequestBehavior.AllowGet);

        }



        public ActionResult StockDetail()
        {

            return View();
        }

        public class modelChartData
        {
            public string INDEX_DATE;
            public string INDEX_POINT;
            public string INDEX_START;
            public string INDEX_HIGH;
            public string INDEX_LOW;
        }

        public ActionResult GetChart()
        {

            DataSet model = modelChart.GetKospi();

            List<modelChartData> List = new List<modelChartData>();

            for (int iloopCount = 0; iloopCount < model.Tables[0].Rows.Count; iloopCount++)
            {
                modelChartData data = new modelChartData();
                data.INDEX_DATE = model.Tables[0].Rows[iloopCount]["INDEX_DATE"].ToString();
                data.INDEX_POINT = model.Tables[0].Rows[iloopCount]["INDEX_POINT"].ToString();
                data.INDEX_START = model.Tables[0].Rows[iloopCount]["INDEX_START"].ToString();
                data.INDEX_HIGH = model.Tables[0].Rows[iloopCount]["INDEX_HIGH"].ToString();
                data.INDEX_LOW = model.Tables[0].Rows[iloopCount]["INDEX_LOW"].ToString();

                List.Add(data);
            }

            return Json(List, JsonRequestBehavior.AllowGet);
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

        public ActionResult GetRealTimeKospi()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            string html = wc.DownloadString("https://finance.naver.com/");
            string json = JsonExport.Between(html, "<div class=\"heading_area\"> ", "</div>");
            json = JsonExport.Between(json, "<span class=\"num\">", "</span>");

            var jsonData = json.Replace(",", "");

            if (jsonData != "")
            {
                modelChart.SetKospiPoint(jsonData);
            }

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
