using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Infra.Connection
{
    public class ConnectionStrings
    {
        public string BD { get; private set; }

        public ConnectionStrings(IConfiguration configuration)
        {
            BD = configuration.GetConnectionString("ConectaBanco");
        }
    }
}
