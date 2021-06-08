using StockPang.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace StockPang.Models
{
    public class modelLogin
    {

        public static bool GetCheckUser(string UserCode, string UserPass)
        {

            string sSql = @"";
            sSql += "   SELECT  COUNT(*) ";
            sSql += "   FROM    USER_MST";
            sSql += "   WHERE   USER_CODE = '" + UserCode + "'";
            sSql += "   AND     PASSWORD  = '" + UserPass + "'";
 
             DataSet ds;

            using (var db = new MSSQLDB())
            {
                ds = db.Query(sSql);

                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        public static DataSet GetUser(string UserCode)
        {

            string sSql = @"";
            sSql += "   SELECT  USER_NAME, USER_ALIAS, USER_STATUS ";
            sSql += "   FROM    USER_MST";
            sSql += "   WHERE   USER_CODE = '" + UserCode + "'";

            DataSet ds;

            using (var db = new MSSQLDB())
            {
                ds = db.Query(sSql);
            }

            return ds;
        }

    }
}