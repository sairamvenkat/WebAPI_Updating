using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAPI.Connections
{
    public static class Connections
    {
        public static SqlConnectionStringBuilder connectToDB()
        {
            var cb = new SqlConnectionStringBuilder();
            cb.DataSource = "sairamvenkatsqlserver.database.windows.net";
            cb.UserID = "sairamvenkat";
            cb.Password = "Microsoft2@";
            cb.InitialCatalog = "onlineshopping";
            cb.MultipleActiveResultSets = true;
            return cb;
        }
    }
}
