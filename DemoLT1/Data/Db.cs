using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLT1.Data
{
    public static class Db
    {
        private static readonly string _connectionString = @"
        Server=localhost;
        Database=lt1;
        Trusted_connection=True;
        TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
