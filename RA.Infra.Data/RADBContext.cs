using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Infra.Data
{
    public class RADBContext
    {
        public static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ResearchAssist;Integrated Security=True";

        public static SqlConnection RADBConnection = new SqlConnection(connectionString);
    }
}
