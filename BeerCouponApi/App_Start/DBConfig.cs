using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BeerCouponApi.App_Start
{
    public class DBConfig
    {
        public static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = BeerCoupons; Integrated Security = True";

        public static Object checkDBNull (Object o)
        {
            return o is DBNull ?  null : o;
        }

        public Object bindDBDATAtoObject(IDataRecord dr, Object o)
        {
            foreach (var property in o.GetType().GetProperties())
            {
                try
                {
                    property.SetValue(o, dr[property.Name]);

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return o;
        }
    }
}