using Microsoft.Extensions.Logging;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain;
using SistemaEscolar.Api.Domain.Entidades;
using SistemaEscolar.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly ILogger<AlunoService> _logger;
        private readonly IAlunoRepository _alunoRepository;
        public AlunoService(ILogger<AlunoService> logger, IAlunoRepository alunoRepository)
        {
            _logger = logger;
            _alunoRepository = alunoRepository;
        }
        public Task<ResultadoOperacao<Aluno.CadastraAluno>> CadastraAluno(Aluno.CadastraAluno parametros)
        {
            return _alunoRepository.CadastraAluno(parametros);
        }
        public Task<ResultadoOperacao<Aluno.DadosAluno>> ConsultaAluno(int Id)
        {
            return _alunoRepository.ConsultaAluno(Id);
        }
        public Task<ResultadoOperacao<List<Aluno.DadosAluno>>> ListaAlunos()
        {
            return _alunoRepository.ListaAlunos();
        }     
        public Task<ResultadoOperacao<Aluno.EditaAluno>> EditaAluno(Aluno.EditaAluno parametros)
        {
            return _alunoRepository.EditaAluno(parametros);
        }
        public Task<ResultadoOperacao<Aluno.DadosAluno>> ExcluiAluno(int Id)
        {
            return _alunoRepository.ExcluiAluno(Id);
        }
        public Task<bool> ValidaLogin(string email, string senha, int nivel)
        {
            return _alunoRepository.ValidaLogin(email, senha, nivel);
        }
        public Task<byte[]> RelatorioPDF(string id)
        {
            return _alunoRepository.RelatorioPDF(id);
        }
        public Task<string> GenerateToken(string id)
        {
            return _alunoRepository.GenerateToken(id);
        }
    }
}
