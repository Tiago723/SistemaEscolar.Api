using SistemaEscolar.Api.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Domain.Interfaces
{
    public interface IRelatoriosRepository
    {
        Task<byte[]> RelatorioPDF(string id);
        Task<string> GenerateToken(string id);
    }
}
