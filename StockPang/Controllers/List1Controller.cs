using StockPang.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPang.Controllers
{
    public class List1Controller : Controller
    {
        //
        // GET: /List1/

        public ActionResult StockList(string Search_name, string Search_class, string Search_psr, string Search_por, string Search_per, string Search_biz_rate, string Search_net_rate)
        {
            DataSet data = null;

            data = modelList1.GetSearchData(Search_name, Search_class, Search_psr, Search_por, Search_per, Search_biz_rate, Search_net_rate, "All", "0");

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

            ViewData["VIEWNAME"] = "StockList";
            return View("StockList", data);
        }
        

    }
}
