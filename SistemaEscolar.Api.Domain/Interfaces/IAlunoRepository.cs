using SistemaEscolar.Api.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Domain.Interfaces
{
    public interface IAlunoRepository
    {
        Task<ResultadoOperacao<Aluno.CadastraAluno>> CadastraAluno(Aluno.CadastraAluno parametros);
        Task<ResultadoOperacao<Aluno.DadosAluno>> ConsultaAluno(int Id);
        Task<ResultadoOperacao<List<Aluno.DadosAluno>>> ListaAlunos();
        Task<ResultadoOperacao<Aluno.EditaAluno>> EditaAluno(Aluno.EditaAluno parametros);
        Task<ResultadoOperacao<Aluno.DadosAluno>> ExcluiAluno(int Id);
        Task<bool> ChecaLoginAluno(string email, string senha);
        Task<byte[]> RelatorioPDF(string id);
        Task<string> GenerateToken(string id);
    }
}
