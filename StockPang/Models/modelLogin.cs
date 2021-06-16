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

        public static bool GetIDCheck(string UserCode)
        {

            string sSql = @"";
            sSql += "   SELECT  COUNT(*) ";
            sSql += "   FROM    USER_MST";
            sSql += "   WHERE   USER_CODE = '" + UserCode + "'";

            DataSet ds;

            using (var db = new MSSQLDB())
            {
                ds = db.Query(sSql);
            }

            if (ds.Tables[0].Rows[0][0].ToString() == "1")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static bool GetAliasCheck(string UserAlias)
        {

            string sSql = @"";
            sSql += "   SELECT  COUNT(*) ";
            sSql += "   FROM    USER_MST";
            sSql += "   WHERE   USER_ALIAS = '" + UserAlias + "'";

            DataSet ds;

            using (var db = new MSSQLDB())
            {
                ds = db.Query(sSql);
            }

            if (ds.Tables[0].Rows[0][0].ToString() == "1")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static int SetUser(string UserCode, string PassWord, string UserAlias, string UserPhone, string UserEmail )
        {

            string sSql = @"";
            sSql += "   INSERT INTO USER_MST ( USER_CODE, PASSWORD, USER_ALIAS, USER_PHONE, USER_EMAIL, USER_STATUS )";
            sSql += "   VALUES  ( '" + UserCode + "', '" + PassWord + "', '" + UserAlias + "', '" + UserPhone + "', '" + UserEmail + "', 'Y')";

            int result;

            using (var db = new MSSQLDB())
            {
                result = db.Execute(sSql);
            }

            return result;
        }

    }
}