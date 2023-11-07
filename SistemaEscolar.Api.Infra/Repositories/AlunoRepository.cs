using Dapper;
using Microsoft.Extensions.Configuration;
using SistemaEscolar.Api.Domain;
using SistemaEscolar.Api.Domain.Entidades;
using SistemaEscolar.Api.Domain.Interfaces;
using SistemaEscolar.Api.Infra.Connection;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SistemaEscolar.Api.Infra.Repositories
{
    public class AlunoRepository : BaseRepository, IAlunoRepository
    {
        private readonly IDbConnection conn;
        public readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _config;

        public AlunoRepository(ConnectionStrings connectionDb, IConfiguration config) : base(connectionDb)
        {
            conn = new SqlConnectionDB().ConnectionDB(_connectionDb.BD);
            _config = config;
        }
 
        public async Task<ResultadoOperacao<Aluno.CadastraAluno>> CadastraAluno(Aluno.CadastraAluno parametros)
        {
            ResultadoOperacao<Aluno.CadastraAluno> resultadoOperacao = new();

            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    conexao.Open();
                    if (conexao.State == ConnectionState.Open)
                    {
                        // Verifica se o aluno já está cadastrado com base no CPF e email
                        string verificaAlunoQuery = "SELECT COUNT(*) FROM alunos WHERE cpf = @cpf AND email = @email";

                        using (SqlCommand verificaAlunoComando = new SqlCommand(verificaAlunoQuery, conexao))
                        {
                            verificaAlunoComando.Parameters.AddWithValue("@cpf", parametros.Cpf);
                            verificaAlunoComando.Parameters.AddWithValue("@email", parametros.Email);

                            int numeroDeAlunosEncontrados = (int)verificaAlunoComando.ExecuteScalar();

                            if (numeroDeAlunosEncontrados > 0)
                            {
                                resultadoOperacao.MensagemRetorno = "Aluno já cadastrado com o mesmo CPF ou email.";
                                resultadoOperacao.ExecutouComSucesso = false;
                                resultadoOperacao.Codigo = "409"; 
                                resultadoOperacao.Detalhe = "Um aluno com o mesmo CPF ou email já está cadastrado.";
                            }
                            else
                            {
                                var SenhaCriptografada = HashPassword(parametros.senha);

                                var query = "INSERT INTO alunos (nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, senha, nivel) VALUES (@nome_aluno, @cpf, @tel, @genero, @estado_civil, @nasc, @cidade_nasc, @estado_nasc, @endereco, @bairro, @cidade, @estado, @numero, @complemento, @cep, @email, @senha, @nivel)";

                                using (SqlCommand comando = new SqlCommand(query, conexao))
                                {
                                    comando.Parameters.AddWithValue("@nome_aluno", parametros.nome_aluno);
                                    comando.Parameters.AddWithValue("@cpf", parametros.Cpf);
                                    comando.Parameters.AddWithValue("@tel", parametros.Tel);
                                    comando.Parameters.AddWithValue("@genero", parametros.Genero);
                                    comando.Parameters.AddWithValue("@estado_civil", parametros.estado_civil);
                                    comando.Parameters.AddWithValue("@nasc", parametros.Nasc);
                                    comando.Parameters.AddWithValue("@cidade_nasc", parametros.cidade_nasc);
                                    comando.Parameters.AddWithValue("@estado_nasc", parametros.estado_nasc);
                                    comando.Parameters.AddWithValue("@endereco", parametros.Endereco);
                                    comando.Parameters.AddWithValue("@bairro", parametros.Bairro);
                                    comando.Parameters.AddWithValue("@cidade", parametros.Cidade);
                                    comando.Parameters.AddWithValue("@estado", parametros.Estado);
                                    comando.Parameters.AddWithValue("@numero", parametros.numero);
                                    comando.Parameters.AddWithValue("@complemento", parametros.complemento);
                                    comando.Parameters.AddWithValue("@cep", parametros.Cep);
                                    comando.Parameters.AddWithValue("@email", parametros.Email);
                                    comando.Parameters.AddWithValue("@senha", SenhaCriptografada);
                                    comando.Parameters.AddWithValue("@nivel", parametros.nivel);

                                    comando.ExecuteNonQuery();
                                    comando.Dispose();
                                }

                                resultadoOperacao.ExecutouComSucesso = true;
                                resultadoOperacao.MensagemRetorno = "A operação executou com sucesso";
                                resultadoOperacao.Codigo = "200";
                                resultadoOperacao.Detalhe = "Cadastro realizado com sucesso.";
                            }
                        }
                    }
                    else
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONECTAR AO BANCO DE DADOS";
                        resultadoOperacao.ExecutouComSucesso = false;
                        resultadoOperacao.Codigo = "500";
                        resultadoOperacao.Detalhe = "A conexão com o banco de dados não pôde ser estabelecida.";
                    }

                    return resultadoOperacao;
                }
                catch (Exception ex)
                {
                    if (resultadoOperacao.resultado == null)
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CADASTRAR O ALUNO";
                    }

                    resultadoOperacao.ExecutouComSucesso = false;
                    resultadoOperacao.Codigo = "500";
                    resultadoOperacao.Detalhe = ex.ToString();

                    return resultadoOperacao;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public async Task<ResultadoOperacao<Aluno.DadosAluno>> ConsultaAluno(int Id)
        {
            ResultadoOperacao<Aluno.DadosAluno> resultadoOperacao = new();
            var Aluno = new Aluno.DadosAluno();

            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    var query = "SELECT cd_aluno, nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, nivel FROM alunos WHERE cd_aluno = '" + Id + "' AND nivel = 2";
                    conexao.Open();

                    if (conexao.State == ConnectionState.Open)
                    {
                        var alunoExistente = await conexao.QueryFirstOrDefaultAsync<Aluno.DadosAluno>("SELECT * FROM alunos WHERE cd_aluno = @cd_aluno", new { cd_aluno = Id });

                        if (alunoExistente == null)
                        {
                            resultadoOperacao.MensagemRetorno = "Aluno não encontrado";
                            return resultadoOperacao;
                        }

                        using (SqlCommand comando = new SqlCommand(query, conexao))
                        {
                            SqlDataReader dr = comando.ExecuteReader();
                            dr.Read();

                            Aluno.cd_aluno = dr.GetInt32(0);
                            Aluno.nome_aluno = dr.GetString(1);
                            Aluno.Cpf = dr.GetString(2);
                            Aluno.Tel = dr.GetString(3);
                            Aluno.Genero = dr.GetString(4);
                            Aluno.estado_civil = dr.GetString(5);
                            Aluno.nasc = dr.GetString(6);
                            Aluno.cidade_nasc = dr.GetString(7);
                            Aluno.estado_nasc = dr.GetString(8);
                            Aluno.Endereco = dr.GetString(9);
                            Aluno.Bairro = dr.GetString(10);
                            Aluno.Cidade = dr.GetString(11);
                            Aluno.Estado = dr.GetString(12);
                            Aluno.numero = dr.GetString(13);
                            Aluno.complemento = dr.GetString(14);
                            Aluno.Cep = dr.GetString(15);
                            Aluno.Email = dr.GetString(16);
                            Aluno.nivel = dr.GetInt32(17);

                            comando.Dispose();

                            dr.Close();
                        }

                        resultadoOperacao.resultado = Aluno;
                        resultadoOperacao.ExecutouComSucesso = true;
                        resultadoOperacao.MensagemRetorno = "A operação executou com sucesso";
                        resultadoOperacao.Codigo = "200";
                        resultadoOperacao.Detalhe = "Consulta realizada com sucesso";
                    }
                    else
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONECTAR AO BANCO DE DADOS";
                        resultadoOperacao.ExecutouComSucesso = false;
                        resultadoOperacao.Codigo = "500";
                        resultadoOperacao.Detalhe = "A conexão com o banco de dados não pôde ser estabelecida.";
                    }

                    return resultadoOperacao;
                }
                catch (Exception ex)
                {
                    if (resultadoOperacao.resultado == null)
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONSULTAR O ALUNO";
                    }

                    resultadoOperacao.ExecutouComSucesso = false;
                    resultadoOperacao.Codigo = "500";
                    resultadoOperacao.Detalhe = ex.ToString();

                    return resultadoOperacao;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public async Task<ResultadoOperacao<List<Aluno.DadosAluno>>> ListaAlunos()
        {
            ResultadoOperacao<List<Aluno.DadosAluno>> resultadoOperacao = new();

            var query = "SELECT COUNT(*) FROM alunos";

            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    conexao.Open();

                    if (conexao.State == ConnectionState.Open)
                    {
                        int alunoCount = await conexao.ExecuteScalarAsync<int>(query, conexao);

                        if (alunoCount == 0)
                        {
                            resultadoOperacao.MensagemRetorno = "NENHUM ALUNO CADASTRADO";
                            return resultadoOperacao;
                        }

                        query = "SELECT cd_aluno, nome_aluno, cpf, tel, genero, estado_civil, nasc, cidade_nasc, estado_nasc, endereco, bairro, cidade, estado, numero, complemento, cep, email, nivel FROM alunos";

                        var lista = (await conexao.QueryAsync<Aluno.DadosAluno>(query, conexao)).ToList();

                        resultadoOperacao.resultado = lista;
                        resultadoOperacao.ExecutouComSucesso = true;
                        resultadoOperacao.MensagemRetorno = "A operação executou com sucesso";
                        resultadoOperacao.Codigo = "200";
                        resultadoOperacao.Detalhe = "Lista retornada com sucesso.";
                    }
                    else
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONECTAR AO BANCO DE DADOS";
                        resultadoOperacao.ExecutouComSucesso = false;
                        resultadoOperacao.Codigo = "500";
                        resultadoOperacao.Detalhe = "A conexão com o banco de dados não pôde ser estabelecida.";
                    }

                    return resultadoOperacao;
                }
                catch (Exception)
                {
                    resultadoOperacao.MensagemRetorno = "FALHA AO RETORNAR A LISTA";
                    throw;
                }
                finally
                {
                    conexao?.Close();
                }
            }
        }

        public async Task<ResultadoOperacao<Aluno.EditaAluno>> EditaAluno(Aluno.EditaAluno parametros)
        {
            ResultadoOperacao<Aluno.EditaAluno> resultadoOperacao = new();

            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    conexao.Open();

                    if (conexao.State == ConnectionState.Open)
                    {
                        var SenhaCriptografada = HashPassword(parametros.senha);

                        var alunoExistente = await conexao.QueryFirstOrDefaultAsync<Aluno.EditaAluno>("SELECT * FROM alunos WHERE cd_aluno = @cd_aluno", new { cd_aluno = parametros.cd_aluno });

                        if (alunoExistente == null)
                        {
                            resultadoOperacao.MensagemRetorno = "Aluno não encontrado";
                            return resultadoOperacao;
                        }

                        // Os campos a serem alterados são conforme a necessidade, sendo assim não é obrigatório todos.
                        var query = "UPDATE alunos SET " +
                            "nome_aluno = @nome_aluno, " +
                            "tel = @Tel, " +
                            "genero = @Genero, " +
                            "estado_civil = @estado_civil, " +
                            "nasc = @nasc, " +
                            "cidade_nasc = @cidade_nasc, " +
                            "estado_nasc = @estado_nasc, " +
                            "endereco = @Endereco, " +
                            "bairro = @Bairro, " +
                            "cidade = @Cidade, " +
                            "estado = @Estado, " +
                            "numero = @numero, " +
                            "complemento = @complemento, " +
                            "cep = @Cep, " +
                            "email = @Email WHERE cd_aluno = @cd_aluno";

                        using (SqlCommand command = new SqlCommand(query, conexao))
                        {
                            command.Parameters.AddWithValue("@nome_aluno", parametros.nome_aluno);
                            command.Parameters.AddWithValue("@Tel", parametros.Tel);
                            command.Parameters.AddWithValue("@Genero", parametros.Genero);
                            command.Parameters.AddWithValue("@estado_civil", parametros.estado_civil);
                            command.Parameters.AddWithValue("@nasc", parametros.nasc);
                            command.Parameters.AddWithValue("@cidade_nasc", parametros.cidade_nasc);
                            command.Parameters.AddWithValue("@estado_nasc", parametros.estado_nasc);
                            command.Parameters.AddWithValue("@Endereco", parametros.Endereco);
                            command.Parameters.AddWithValue("@Bairro", parametros.Bairro);
                            command.Parameters.AddWithValue("@Cidade", parametros.Cidade);
                            command.Parameters.AddWithValue("@Estado", parametros.Estado);
                            command.Parameters.AddWithValue("@numero", parametros.numero);
                            command.Parameters.AddWithValue("@complemento", parametros.complemento);
                            command.Parameters.AddWithValue("@Cep", parametros.Cep);
                            command.Parameters.AddWithValue("@Email", parametros.Email);
                            command.Parameters.AddWithValue("@cd_aluno", parametros.cd_aluno);

                            command.ExecuteNonQuery();

                            resultadoOperacao.ExecutouComSucesso = true;
                            resultadoOperacao.MensagemRetorno = "A operação executou com sucesso";
                            resultadoOperacao.Codigo = "200";
                            resultadoOperacao.Detalhe = "Dados atualizados com sucesso.";
                        }
                    }
                    else
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONECTAR AO BANCO DE DADOS";
                        resultadoOperacao.ExecutouComSucesso = false;
                        resultadoOperacao.Codigo = "500";
                        resultadoOperacao.Detalhe = "A conexão com o banco de dados não pôde ser estabelecida.";
                    }

                    return resultadoOperacao;
                }
                catch (Exception ex)
                {
                    resultadoOperacao.MensagemRetorno = "FALHA AO EDITAR DADOS DO ALUNO";
                    resultadoOperacao.ExecutouComSucesso = false;
                    resultadoOperacao.Codigo = "500";
                    resultadoOperacao.Detalhe = ex.ToString();
                    return resultadoOperacao;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public async Task<ResultadoOperacao<Aluno.DadosAluno>> ExcluiAluno(int Id)
        {
            ResultadoOperacao<Aluno.DadosAluno> resultadoOperacao = new();
            var Aluno = new Aluno.DadosAluno();

            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    conexao.Open();

                    if (conexao.State == ConnectionState.Open)
                    {
                        var alunoExistente = await conexao.QueryFirstOrDefaultAsync<Aluno.DadosAluno>("SELECT * FROM alunos WHERE cd_aluno = @cd_aluno", new { cd_aluno = Id });

                        if (alunoExistente == null)
                        {
                            resultadoOperacao.MensagemRetorno = "Aluno não encontrado";
                            return resultadoOperacao;
                        }

                        var query = "DELETE FROM alunos WHERE cd_aluno= '" + Id + "'";

                        using (SqlCommand command = new SqlCommand(query, conexao))
                        {
                            command.ExecuteNonQuery();

                            resultadoOperacao.ExecutouComSucesso = true;
                            resultadoOperacao.MensagemRetorno = "A operação executou com sucesso";
                            resultadoOperacao.Codigo = "200";
                            resultadoOperacao.Detalhe = "Aluno excluído com sucesso.";
                        }
                    }
                    else
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO CONECTAR AO BANCO DE DADOS";
                        resultadoOperacao.ExecutouComSucesso = false;
                        resultadoOperacao.Codigo = "500";
                        resultadoOperacao.Detalhe = "A conexão com o banco de dados não pôde ser estabelecida.";
                    }

                    return resultadoOperacao;
                }
                catch (Exception ex)
                {
                    if (resultadoOperacao.resultado == null)
                    {
                        resultadoOperacao.MensagemRetorno = "FALHA AO EXCLUIR O ALUNO";
                    }

                    resultadoOperacao.ExecutouComSucesso = false;
                    resultadoOperacao.Codigo = "500";
                    resultadoOperacao.Detalhe = ex.ToString();

                    return resultadoOperacao;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
