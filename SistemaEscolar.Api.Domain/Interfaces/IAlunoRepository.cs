﻿using SistemaEscolar.Api.Domain.Entidades;
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
        Task<ResultadoOperacao<Aluno.DadosAluno>> ConsultaAluno(string email);
        Task<ResultadoOperacao<List<Aluno.DadosAluno>>> ListaAlunos();
        Task<ResultadoOperacao<Aluno.EditaAluno>> EditaAluno(Aluno.EditaAluno parametros);
        Task<ResultadoOperacao<Aluno.DadosAluno>> ExcluiAluno(int Id);
    }
}
