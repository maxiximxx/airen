using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace airen.Controllers
{
    public class DB
    {
        public static MySqlConnection MBT()
        {
            return new MySqlConnection("server=127.0.0.1;uid=root;pwd=111;persistsecurityinfo=True;database=airen;Allow Zero Datetime=True");
        }
    }
}