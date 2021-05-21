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
            sSql += "   SELECT  TOP 100	INDEX_DATE, ISNULL(INDEX_POINT,0) INDEX_POINT,	ISNULL(INDEX_START,0) INDEX_START,	ISNULL(INDEX_HIGH,0) INDEX_HIGH, ISNULL(INDEX_LOW,0) INDEX_LOW";
            sSql += "   FROM	INDEX_POINT";
            sSql += "   WHERE	STOCK_MARKET	=	'KOSPI'";
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

            string sSql = @"";
            sSql += " SELECT ROUND(AVG(INDEX_POINT),2) AVG_POINT ";
            sSql += " FROM   ( " ; 
            sSql += "        SELECT  TOP 45	ISNULL(INDEX_POINT,0) INDEX_POINT";
            sSql += "        FROM	INDEX_POINT";
            sSql += "        WHERE	STOCK_MARKET	=	'KOSPI'";
            sSql += "        ORDER BY INDEX_DATE DESC ";
            sSql += "        ) A";

            DataSet user;

            using (var db = new MSSQLDB())
            {
                user = db.Query(sSql);
            }

            return user;
        }



    }
}