using SistemaEscolar.Api.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SistemaEscolar.Api.Domain.Entidades.Aluno;

namespace SistemaEscolar.Api.Domain.Interfaces
{
    public interface IRelatoriosRepository
    {
        Task<byte[]> RelatorioAluno(string token);
        Task<string> VerificaAluno(string id);
    }
}
