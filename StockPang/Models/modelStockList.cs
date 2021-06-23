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

        public static DataSet GetSearchData2(
                                            string Search_name, string Search_class, string Search_per, string Search_gap
                                        ,   string Search_sa_rate, string Search_bp_rate, string Search_np_rate
                                        ,   string List_Type, string User_ID
            )
        {

            string sSql = @"";
            sSql += "   SELECT  A.STOCK_CODE, A.STOCK_NAME, STOCK_URL, TOTAL_AMT, SALES_AMT, BIZ_PROFIT, NET_PROFIT, PER, EST_PER, BIZ_PER, STOCK_PRICE, NAVER_PRICE, ";
            sSql += "           CAL_PSR, CAL_POR, CAL_PER, BIZ_RATE, NET_RATE, ";
            sSql += "           SA_Y3, SA_Y2, SA_Y1, SA_Y0, BP_Y3, BP_Y2, BP_Y1, BP_Y0, NP_Y3, NP_Y2, NP_Y1, NP_Y0, ";
            sSql += "           SA_RATE, BP_RATE, NP_RATE, ";
            sSql += "           B.STOCK_REMARK, ISNULL(LEN(B.STOCK_REMARK),0) as REMARK_LEN ";
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

        public static DataSet GetSearchData3(
                                            string Search_name, string Search_class, string Search_buy_gap, string Search_sell_gap
                                        , string List_Type, string User_ID
            )
        {

            string sSql = @"";
            sSql += "   SELECT  A.STOCK_CODE, B.STOCK_NAME, B.STOCK_URL, B.STOCK_CLASS1, B.STOCK_PRICE, B.NAVER_PRICE, ";
            sSql += "           A.BUY_PRICE, A.SELL_PRICE, A.AVG_PRICE,  ";
            sSql += "           A.STOCK_REMARK, ISNULL(LEN(A.STOCK_REMARK),0) as REMARK_LEN, ";
            sSql += "           A.BUY_PRICE - B.STOCK_PRICE as BUY_GAP, ";
            sSql += "           ROUND(CASE A.BUY_PRICE WHEN 0 THEN 0 ELSE (A.BUY_PRICE - B.STOCK_PRICE) / A.BUY_PRICE * 100 END, 2) as BUY_GAP_RATE, ";
            sSql += "           A.SELL_PRICE - B.STOCK_PRICE as SELL_GAP, ";
            sSql += "           ROUND(CASE A.SELL_PRICE WHEN 0 THEN 0 ELSE (A.SELL_PRICE - B.STOCK_PRICE) / A.SELL_PRICE * 100 END, 2) as SELL_GAP_RATE, ";
            sSql += "           ROUND(CASE A.AVG_PRICE WHEN 0 THEN 0 ELSE (B.STOCK_PRICE - A.AVG_PRICE) / A.AVG_PRICE * 100 END,2) as AVG_GAP, ";
            sSql += "           (   SELECT   COUNT(*) REG_CNT ";
            sSql += "               FROM     USER_STOCK	C    ";
            sSql += "               WHERE    C.STOCK_CODE   = A.STOCK_CODE ) REG_CNT, ";
            sSql += "           (   SELECT   ISNULL(AVG(C.BUY_PRICE),0) BUY_AVG ";
            sSql += "               FROM     USER_STOCK	C    ";
            sSql += "               WHERE    C.STOCK_CODE   = A.STOCK_CODE  AND C.BUY_PRICE > 0 ) BUY_AVG, ";
            sSql += "           (   SELECT   ISNULL(AVG(C.SELL_PRICE),0) SELL_PRICE ";
            sSql += "               FROM     USER_STOCK	C    ";
            sSql += "               WHERE    C.STOCK_CODE   = A.STOCK_CODE  AND C.SELL_PRICE > 0 ) SELL_AVG, ";
            sSql += "           (   SELECT   ISNULL(AVG(C.AVG_PRICE),0) AVG_AVG ";
            sSql += "               FROM     USER_STOCK	C    ";
            sSql += "               WHERE    C.STOCK_CODE   = A.STOCK_CODE AND C.AVG_PRICE > 0) AVG_AVG ";
    
            sSql += "   FROM    USER_STOCK  A"; 
            sSql += "       ,   STOCK_INFO  B";
            sSql += "   WHERE   A.USER_ID    = " + User_ID ;      
            sSql += "   AND     B.STOCK_CODE = A.STOCK_CODE ";
            sSql += "   AND   ( B.STOCK_CODE LIKE '%" + Search_name + "%'" + " OR B.STOCK_NAME LIKE '%" + Search_name + "%')";
            sSql += "   AND   ( B.STOCK_CLASS1 LIKE '%" + Search_class + "%'" + " OR B.STOCK_CLASS2 LIKE '%" + Search_class + "%')";

            if (!string.IsNullOrEmpty(Search_buy_gap) && Search_buy_gap != "")
            {
                sSql += "   AND   (A.BUY_PRICE - B.STOCK_PRICE) / A.BUY_PRICE * 100  <= " + Search_buy_gap ;
                sSql += "   AND   A.BUY_PRICE <> 0 ";
            }

            if (!string.IsNullOrEmpty(Search_sell_gap) && Search_sell_gap != "")
            {
                sSql += "   AND   (A.SELL_PRICE - B.STOCK_PRICE) / A.SELL_PRICE * 100  >= " + Search_sell_gap ;
                sSql += "   AND   A.SELL_PRICE <> 0 ";
            }
            sSql += "   ORDER BY B.TOTAL_AMT DESC";


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

        public static bool SetStockInsert(string Reg_Code, string Reg_Name, string Reg_Class1, string Reg_Class2, string Reg_Remark, string User_ID)
        {
            int Result = 0;

            string sSql = @"";
            sSql += "   MERGE INTO STOCK_INFO A  ";
            sSql += "   USING  ( SELECT '" + Reg_Code + "' STOCK_CODE ) B";
            sSql += "   ON      (A.STOCK_CODE = B.STOCK_CODE )";
            sSql += "   WHEN MATCHED THEN	";
            sSql += "   UPDATE";
            sSql += "   SET	STOCK_CODE = '" + Reg_Code + "'";
            sSql += "     , STOCK_NAME = '" + Reg_Name + "'";
            sSql += "     , UPDATE_USER = " + User_ID ;
            sSql += "   WHEN NOT MATCHED THEN 	";
            sSql += "   INSERT (STOCK_CODE, STOCK_NAME, CREATE_USER)";
            sSql += "   VALUES	('" + Reg_Code + "', '" + Reg_Name + "' , " + User_ID + " );";

            int result;

            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }

            sSql = @"";
            sSql += "   UPDATE STOCK_CLASS  ";
            sSql += "   SET    STOCK_REMARK = '" + Reg_Remark + "',"; 
            sSql += "          STOCK_CLASS1 = '" + Reg_Class1 + "',";
            sSql += "          STOCK_CLASS2 = '" + Reg_Class2 + "'";
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
            
            // USER_STOCK 등록 
            if (! (string.IsNullOrEmpty(User_ID) || (User_ID == "0")))
            {
                sSql = @"";
                sSql += "   MERGE INTO USER_STOCK A  ";
                sSql += "   USING  ( SELECT '" + Reg_Code + "' STOCK_CODE, " + User_ID + " USER_ID  ) B";
                sSql += "   ON      (A.STOCK_CODE = B.STOCK_CODE AND A.USER_ID = B.USER_ID )";
                sSql += "   WHEN MATCHED THEN	";
                sSql += "   UPDATE";
                sSql += "   SET	STOCK_REMARK = '" + Reg_Remark + "'";
                sSql += "   WHEN NOT MATCHED THEN 	";
                sSql += "   INSERT (STOCK_CODE, STOCK_NAME, USER_ID, STOCK_REMARK )";
                sSql += "   VALUES	('" + Reg_Code + "', " + Reg_Name + "', " + User_ID + ", '" + Reg_Remark + "' );";

                using (var db = new MSSQLDB())
                {
                    result = db.Execute(sSql);
                }
            }

            if (Result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int SetRemarkSave(string Stock_Code, string Stock_Remark)
        {

            string sSql = @"";
            sSql += "   UPDATE STOCK_CLASS ";
            sSql += "     SET  STOCK_REMARK = '" + Stock_Remark + "'";
            sSql += "   WHERE STOCK_CODE = '" + Stock_Code + "'";

            int Result;

            using (var db = new MSSQLDB())
            {
                Result = db.Execute(sSql);
            }

            return Result;

        }

        public static int SetStockDelete(string Stock_Code)
        {

            string sSql = @"";
            sSql += "   DELETE FROM STOCK_INFO ";
            sSql += "   WHERE STOCK_CODE = '" + Stock_Code + "'";

            int Result;

            using (var db = new MSSQLDB())
            {
                Result = db.Execute(sSql);
            }

            return Result;

        }

        public static bool SetRegDelete(string Stock_Code, string User_ID)
        {

            string sSql = @"";
            sSql += "   DELETE FROM USER_STOCK ";
            sSql += "   WHERE STOCK_CODE = '" + Stock_Code + "'";
            sSql += "   AND   USER_ID = " + User_ID;

            int Result;

            using (var db = new MSSQLDB())
            {
                Result = db.Execute(sSql);
            }


            if (Result > 0) {
                return true;
            }
            else
	        {
               return false;
	        }
 
        }

        public static bool SetSavePrice(string Stock_Code, string User_ID, string Buy_Price, string Sell_Price, string Avg_Price)
        {

            string sSql = @"";
            sSql += "   UPDATE  USER_STOCK ";
            sSql += "   SET     BUY_PRICE = " +  Buy_Price;
            sSql += "       ,   SELL_PRICE = " +  Sell_Price;
            sSql += "       ,   AVG_PRICE = " + Avg_Price;
            sSql += "   WHERE   STOCK_CODE = '" + Stock_Code + "'";
            sSql += "   AND     USER_ID = " + User_ID;

            int Result;

            using (var db = new MSSQLDB())
            {
                Result = db.Execute(sSql);
            }

            return true;

        }


    }


}