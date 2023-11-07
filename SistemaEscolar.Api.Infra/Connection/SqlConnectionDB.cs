using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Infra.Connection
{
    public class SqlConnectionDB
    {
        public IDbConnection ConnectionDB(string conn)
        {
            return new SqlConnection(conn);
        }
    }
}
