using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockPang.DataBase;
using System.Data;

namespace StockPang.Models
{
    public class modelList1
    {
        public static DataSet GetSearchData(string Search_name, string Search_class, string Search_psr
                                            , string Search_por, string Search_per, string Search_biz_rate, string Search_net_rate
                                            , string List_Type, string User_ID
                                        )
        {

            string sSql = @"";
            sSql += "   SELECT  A.STOCK_CODE, A.STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, ";
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0, ";
            sSql += "           SA_RATE, BP_RATE, NP_RATE ,";
            sSql += "           B.STOCK_REMARK, ISNULL(LEN(B.STOCK_REMARK),0) as REMARK_LEN  ";
            sSql += "   FROM    STOCK_INFO  A";
            sSql += "       ,   STOCK_CLASS B";
            sSql += "   WHERE   A.STOCK_CODE = B.STOCK_CODE ";
            sSql += "   AND   ( A.STOCK_CODE LIKE '%" + Search_name + "%'" + " OR A.STOCK_NAME LIKE '%" + Search_name + "%')";
            sSql += "   AND   ( A.STOCK_CLASS1 LIKE '%" + Search_class + "%'" + " OR A.STOCK_CLASS2 LIKE '%" + Search_class + "%')";

            //User별 
            if (List_Type == "User")
            {
                sSql += "   AND   A.STOCK_CODE IN  ( SELECT C.STOCK_CODE FROM USER_STOCK C WHERE C.USER_ID = " + User_ID + " )";
            }

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

    }


}