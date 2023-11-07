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
    public class AutenticacaoRepository : BaseRepository, IAutenticacaoRepository
    {
        private readonly IDbConnection conn;
        public readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _config;

        public AutenticacaoRepository(ConnectionStrings connectionDb, IConfiguration config) : base(connectionDb)
        {
            conn = new SqlConnectionDB().ConnectionDB(_connectionDb.BD);
            _config = config;
        }

        public async Task<bool> ValidaLogin(string usuario, string senha, int nivel)
        {
            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    conexao.Open();
                    if (conexao.State == ConnectionState.Open)
                    {
                        var query = "SELECT senha FROM alunos WHERE email = @usuario AND nivel = '"+nivel+"'";

                        using (var comando = new SqlCommand(query, conexao))
                        {
                            comando.Parameters.AddWithValue("@usuario", usuario);

                            using (var leitor = comando.ExecuteReader())
                            {
                                if (leitor.Read())
                                {
                                    string senhaArmazenada = leitor["senha"] as string;

                                    if (!string.IsNullOrEmpty(senhaArmazenada))
                                    {
                                        if (BCrypt.Net.BCrypt.Verify(senha, senhaArmazenada))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conexao?.Close();
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
