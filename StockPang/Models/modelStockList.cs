using StockPang.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StockPang.Models
{
    public class modelStockList
    {
        public static DataSet GetAllData()
        {

            string sSql = @"";
            sSql += "   SELECT  STOCK_CODE, STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, "; 
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0";
            sSql += "   from    STOCK_INFO";
            sSql += "   ORDER BY STOCK_INFO_ID";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }
        public static DataSet GetSearchData(string Serch_text, string Serch_psr, string Serch_por, string Serch_per, string Serch_biz_rate, string Serch_net_rate)
        {

            string sSql = @"";
            sSql += "   SELECT  STOCK_CODE, STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, ";
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0"; 
            sSql += "   FROM    STOCK_INFO";
            sSql += "   WHERE ( STOCK_CODE LIKE '%" + Serch_text + "%'" + " OR STOCK_NAME LIKE '%" + Serch_text + "%')";
            if (!string.IsNullOrEmpty(Serch_psr) && Serch_psr != "")
            { 
                sSql += "   AND     CAL_PSR   <= " + Serch_psr; 
            }
            if (!string.IsNullOrEmpty(Serch_por) && Serch_por != "")
            { 
                sSql += "   AND     CAL_POR   <= " + Serch_por; 
            }
            if (!string.IsNullOrEmpty(Serch_per) && Serch_per != "")
            { 
                sSql += "   AND     CAL_PER   <= " + Serch_per; 
            }
            if (!string.IsNullOrEmpty(Serch_biz_rate) && Serch_biz_rate != "")
            { 
                sSql += "   AND     BIZ_RATE   >= " + Serch_biz_rate; 
            }
            if (!string.IsNullOrEmpty(Serch_net_rate) && Serch_net_rate != "")
            {
                sSql += "   AND     NET_RATE   >= " + Serch_net_rate; 
            }
            sSql += "   ORDER BY STOCK_INFO_ID";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }
    }
}