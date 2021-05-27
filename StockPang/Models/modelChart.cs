using StockPang.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StockPang.Models
{
    public class modelChart
    {
        public static DataSet GetKospi()
        {

            string sSql = @"";
            sSql += "   SELECT  TOP 300	INDEX_DATE, ISNULL(INDEX_POINT,0) INDEX_POINT,	ISNULL(INDEX_START,0) INDEX_START,	ISNULL(INDEX_HIGH,0) INDEX_HIGH, ISNULL(INDEX_LOW,0) INDEX_LOW";
            sSql += "   FROM	INDEX_POINT";
            sSql += "   WHERE	STOCK_MARKET	=	'KOSPI'";
            sSql += "   AND 	INDEX_DATE 	    >	GETDATE() - 600 ";
            sSql += "   ORDER BY INDEX_DATE ";

            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }

        public static DataSet AvgKospi()
        {

            string YD_POINT;  

            // 어제 날짜 지수 가져오기 
            string sSql = @"";
            sSql += " SELECT MAX(INDEX_POINT) INDEX_POINT ";
            sSql += " FROM   INDEX_POINT A ";
            sSql += " WHERE  STOCK_MARKET	=	'KOSPI'";
            sSql += " AND    INDEX_DATE = ( "; 
            sSql += "       SELECT MAX(INDEX_DATE) INDEX_DATE ";
            sSql += "       FROM   INDEX_POINT A ";
            sSql += "       WHERE  STOCK_MARKET	=	'KOSPI'";
            sSql += "       AND    INDEX_DATE <  "; 
            sSql += "           (       SELECT  TOP 1	INDEX_DATE";
            sSql += "                   FROM	INDEX_POINT";
            sSql += "                   WHERE	STOCK_MARKET	=	'KOSPI'";
            sSql += "                   AND 	INDEX_DATE 	    >	GETDATE() - 10 ";
            sSql += "                   ORDER BY INDEX_DATE DESC ";
            sSql += "           ) ";
            sSql += "      ) ";
            DataSet user;
            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            YD_POINT = user.Tables[0].Rows[0][0].ToString();

            //평균 지수 가져오기 
            sSql = @"";
            sSql += " SELECT ROUND(AVG(INDEX_POINT),2) AVG_POINT ";
            sSql += " FROM   ( " ; 
            sSql += "        SELECT  TOP 60	ISNULL(INDEX_POINT,0) INDEX_POINT";
            sSql += "        FROM	INDEX_POINT";
            sSql += "        WHERE	STOCK_MARKET	=	'KOSPI'";
            sSql += "        AND 	INDEX_DATE 	    >	GETDATE() - 120 ";
            sSql += "        ORDER BY INDEX_DATE DESC ";
            sSql += "        ) A";
            sSql += " UNION ALL";
            sSql += " SELECT " + YD_POINT + " AVG_POINT " ;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }

        public static int SetKospiPoint(string Kospi_Point)
        {

            string sSql;
            int result;
            string YD_POINT;  

            // 어제 날짜 지수 가져오기 
            sSql = @"";
            sSql += " SELECT MAX(INDEX_POINT) INDEX_POINT ";
            sSql += " FROM   INDEX_POINT A ";
            sSql += " WHERE  STOCK_MARKET	=	'KOSPI'";
            sSql += " AND    INDEX_DATE = ( ";
            sSql += "       SELECT MAX(INDEX_DATE) INDEX_DATE ";
            sSql += "       FROM   INDEX_POINT A ";
            sSql += "       WHERE  STOCK_MARKET	=	'KOSPI'";
            sSql += "       AND    INDEX_DATE <  ";
            sSql += "           (       SELECT  TOP 1	INDEX_DATE";
            sSql += "                   FROM	INDEX_POINT";
            sSql += "                   WHERE	STOCK_MARKET	=	'KOSPI'";
            sSql += "                   AND 	INDEX_DATE 	    >	GETDATE() - 10 ";
            sSql += "                   ORDER BY INDEX_DATE DESC ";
            sSql += "           ) ";
            sSql += "      ) ";
            DataSet user;
            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            YD_POINT = user.Tables[0].Rows[0][0].ToString();

            sSql = @"";
            sSql += "   MERGE INTO INDEX_POINT A  ";
            sSql += "   USING  ( SELECT CONVERT(CHAR(10),GETDATE(),23) INDEX_DATE, 'KOSPI' STOCK_MARKET ) B";
            sSql += "   ON      (A.STOCK_MARKET = B.STOCK_MARKET AND A.INDEX_DATE = B.INDEX_DATE )";
            sSql += "   WHEN MATCHED THEN	";
            sSql += "   UPDATE";
            sSql += "   SET	INDEX_POINT = " + Kospi_Point.ToString();
            sSql += "     , INDEX_HIGH  = " + Kospi_Point.ToString();
            sSql += "     , INDEX_START  = " + YD_POINT;
            sSql += "     , INDEX_LOW  = " + YD_POINT;
            sSql += "   WHEN NOT MATCHED THEN 	";
            sSql += "   INSERT (STOCK_MARKET, INDEX_DATE, INDEX_POINT, INDEX_START, INDEX_LOW, INDEX_HIGH, CREATE_DATE)";
            sSql += "   VALUES	('KOSPI',  CONVERT(CHAR(10), GETDATE(), 23),  " + Kospi_Point.ToString() + ", " + YD_POINT + ", " + YD_POINT + ", " + Kospi_Point.ToString() + ", GETDATE() );";

            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }

            return result;
        }

    }
}