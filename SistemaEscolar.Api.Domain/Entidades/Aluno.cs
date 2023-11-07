using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Domain.Entidades
{
    public class Aluno
    {
        public class DadosAluno
        {
            public int cd_aluno { get; set; }
            public string nome_aluno { get; set; }
            public string? Cpf { get; set; }
            public string? Tel { get; set; }
            public string Genero { get; set; }
            public string estado_civil { get; set; }
            public string nasc { get; set; }
            public string cidade_nasc { get; set; }
            public string estado_nasc { get; set; }
            public string Endereco { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string numero { get; set; }
            public string? complemento { get; set; }
            public string Cep { get; set; }
            public string? Email { get; set; }
            public int? nivel { get; set; }
        }

        public class CadastraAluno
        {
            public string nome_aluno { get; set; }
            public string Cpf { get; set; }
            public string? Tel { get; set; }
            public string Genero { get; set; }
            public string estado_civil { get; set; }
            public string Nasc { get; set; }
            public string cidade_nasc { get; set; }
            public string estado_nasc { get; set; }
            public string Endereco { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string numero { get; set; }
            public string? complemento { get; set; }
            public string Cep { get; set; }
            public string Email { get; set; }
            public string senha { get; set; }
            public int nivel { get; set; }
        }

        public class EditaAluno
        {
            public int cd_aluno { get; set; }
            public string nome_aluno { get; set; }
            public string? Tel { get; set; }
            public string Genero { get; set; }
            public string estado_civil { get; set; }
            public string nasc { get; set; }
            public string cidade_nasc { get; set; }
            public string estado_nasc { get; set; }
            public string Endereco { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string numero { get; set; }
            public string? complemento { get; set; }
            public string Cep { get; set; }
            public string? Email { get; set; }
            public string senha { get; set; }
            public int? nivel { get; set; }
        }

        public class ResponsavelDoAluno
        {
            public string? Nome { get; set; }
            public string? Tel { get; set; }
            public string? Cpf { get; set; }
            public string? Genero { get; set; }
            public string? EstadoCivil { get; set; }
            public string? Nascimento { get; set; }
            public string? CidadeNasc { get; set; }
            public string? EstadoNasc { get; set; }
            public string? Endereco { get; set; }
            public string? Bairro { get; set; }
            public string? Cidade { get; set; }
            public string? Estado { get; set; }
            public string? Cep { get; set; }
            public string? Email { get; set; }
            public string? senha { get; set; }
            public string? FotoResponsavel { get; set; }
            public string? Categoria { get; set; }
        }

        public class CredenciaisReportService
        {
            public string username = "";
            public string password = "";
        }
    }
}
