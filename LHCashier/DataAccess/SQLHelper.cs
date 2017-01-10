using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCashier.DataAccess
{
    public static class SQLHelper
    {
        /// <summary>
        /// 获得一个SQL连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string connectionString = ConfigurationManager.ConnectionStrings["azureConnectionString"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine("Connected");
            return connection;
        }
    }
}
