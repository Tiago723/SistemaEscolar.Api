using SistemaEscolar.Api.Infra.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Infra.Repositories
{
    public abstract class BaseRepository
    {
        public ConnectionStrings _connectionDb;
        public BaseRepository(ConnectionStrings connectionDb)
        {
            _connectionDb = connectionDb;
        }
    }
}
