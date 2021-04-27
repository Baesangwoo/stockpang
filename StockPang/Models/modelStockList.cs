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
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0, ";
            sSql += "           SA_RATE, BP_RATE, NP_RATE ";
            sSql += "   FROM    STOCK_INFO";
            sSql += "   ORDER BY TOTAL_AMT DESC";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }
        public static DataSet GetSearchData(string Search_name, string Search_class, string Search_psr, string Search_por, string Search_per, string Search_biz_rate, string Search_net_rate)
        {

            string sSql = @"";
            sSql += "   SELECT  STOCK_CODE, STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, ";
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0, ";
            sSql += "           SA_RATE, BP_RATE, NP_RATE ";
            sSql += "   FROM    STOCK_INFO";
            sSql += "   WHERE ( STOCK_CODE LIKE '%" + Search_name + "%'" + " OR STOCK_NAME LIKE '%" + Search_name + "%')";
            sSql += "   AND   ( STOCK_CLASS1 LIKE '%" + Search_class + "%'" + " OR STOCK_CLASS2 LIKE '%" + Search_class + "%')"; 
            if (!string.IsNullOrEmpty(Search_psr) && Search_psr != "")
            {
                sSql += "   AND     CAL_PSR   <= " + Search_psr;
            }
            if (!string.IsNullOrEmpty(Search_por) && Search_por != "")
            {
                sSql += "   AND     CAL_POR   <= " + Search_por;
            }
            if (!string.IsNullOrEmpty(Search_per) && Search_per != "")
            {
                sSql += "   AND     CAL_PER   <= " + Search_per;
            }
            if (!string.IsNullOrEmpty(Search_biz_rate) && Search_biz_rate != "")
            {
                sSql += "   AND     BIZ_RATE   >= " + Search_biz_rate;
            }
            if (!string.IsNullOrEmpty(Search_net_rate) && Search_net_rate != "")
            {
                sSql += "   AND     NET_RATE   >= " + Search_net_rate;
            }
            sSql += "   ORDER BY TOTAL_AMT DESC";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }

        public static DataSet GetSearchData2(string Search_name, string Search_class, string Search_per, string Search_gap, string Search_sa_rate, string Search_bp_rate, string Search_np_rate)
        {

            string sSql = @"";
            sSql += "   SELECT  STOCK_CODE, STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, ";
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0, ";
            sSql += "           SA_RATE, BP_RATE, NP_RATE ";
            sSql += "   FROM    STOCK_INFO";
            sSql += "   WHERE ( STOCK_CODE LIKE '%" + Search_name + "%'" + " OR STOCK_NAME LIKE '%" + Search_name + "%')";
            sSql += "   AND   ( STOCK_CLASS1 LIKE '%" + Search_class + "%'" + " OR STOCK_CLASS2 LIKE '%" + Search_class + "%')"; 
            if (!string.IsNullOrEmpty(Search_per) && Search_per != "")
            {
                sSql += "   AND     CAL_PER   <= " + Search_per;
            }

            if (!string.IsNullOrEmpty(Search_gap) && Search_gap != "")
            {
                sSql += "   AND  (  (NAVER_PRICE - STOCK_PRICE) / NAVER_PRICE * 100  >= " + Search_gap + " OR  (NAVER_PRICE - STOCK_PRICE) / NAVER_PRICE * 100 <=  (" + Search_gap + "*-1)  )";
                sSql += "   AND   NAVER_PRICE <> 0 ";
            }

            if (!string.IsNullOrEmpty(Search_sa_rate) && Search_sa_rate != "")
            {
                sSql += "   AND     SA_RATE   >= " + Search_sa_rate;
            }
            if (!string.IsNullOrEmpty(Search_bp_rate) && Search_bp_rate != "")
            {
                sSql += "   AND     BP_RATE   >= " + Search_bp_rate;
            }
            if (!string.IsNullOrEmpty(Search_np_rate) && Search_np_rate != "")
            {
                sSql += "   AND     NP_RATE   >= " + Search_np_rate;
            }
            sSql += "   ORDER BY TOTAL_AMT DESC";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }


        public static DataSet GetSearchReg(string Search_name, string Search_class)
        {

            string sSql = @"";
            sSql += "   SELECT  STOCK_CODE, STOCK_NAME, STOCK_CLASS1, STOCK_CLASS2, STOCK_REMARK ";
            sSql += "   FROM    STOCK_CLASS";
            sSql += "   WHERE   1   =   1 ";
            if (string.IsNullOrEmpty(Search_name) && string.IsNullOrEmpty(Search_class) )
            {
                sSql += "   AND 1   =   2 ";
            }
            else
            {
                sSql += "   AND  ( STOCK_CODE LIKE '%" + Search_name + "%'" + " OR STOCK_NAME LIKE '%" + Search_name + "%')";
                sSql += "   AND  ( STOCK_CLASS1 LIKE '%" + Search_class + "%'" + " OR STOCK_CLASS2 LIKE '%" + Search_class + "%')";
            }
            sSql += "   ORDER BY STOCK_NAME";


            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;

        }

        public static int SetStockInsert(string Reg_Code, string Reg_Name, string Reg_Remark)
        {

            string sSql = @"";
            sSql += "   MERGE INTO STOCK_INFO A  ";
            sSql += "   USING  ( SELECT '" + Reg_Code + "' STOCK_CODE ) B";
            sSql += "   ON      (A.STOCK_CODE = B.STOCK_CODE )";
            sSql += "   WHEN MATCHED THEN	";
            sSql += "   UPDATE";
            sSql += "   SET	STOCK_CODE = '" + Reg_Code + "'";
            sSql += "     , STOCK_NAME = '" + Reg_Name + "'";
            sSql += "   WHEN NOT MATCHED THEN 	";
            sSql += "   INSERT (STOCK_CODE, STOCK_NAME)";
            sSql += "   VALUES	('" + Reg_Code + "', '" + Reg_Name + "');";

            int result;

            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }

            sSql = @"";
            sSql += "   UPDATE STOCK_CLASS  ";
            sSql += "   SET    STOCK_REMARK = '" + Reg_Remark + "'"; ;
            sSql += "   FROM   STOCK_CLASS A ";
            sSql += "   WHERE   A.STOCK_CODE = '" + Reg_Code + "'";

            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }
            


            sSql = @"";
            sSql += "   UPDATE STOCK_INFO  ";
            sSql += "   SET    STOCK_CLASS1 = B.STOCK_CLASS1, ";
            sSql += "          STOCK_CLASS2 = B.STOCK_CLASS2  ";
            sSql += "   FROM   STOCK_INFO  A ";
            sSql += "       ,  STOCK_CLASS B ";
            sSql += "   WHERE   A.STOCK_CODE = '" + Reg_Code + "'";
            sSql += "   AND     B.STOCK_CODE = A.STOCK_CODE 	";
            
            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }
            
            return result;

        }

        public static DataSet SetStockDelete(string Stock_Code)
        {

            string sSql = @"";
            sSql += "   DELETE FROM STOCK_INFO ";
            sSql += "   WHERE STOCK_CODE = '" + Stock_Code + "'";

            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;

        }


    }


}