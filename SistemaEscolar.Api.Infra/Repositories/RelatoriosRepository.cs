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
    public class RelatoriosRepository : BaseRepository, IRelatoriosRepository
    {
        private readonly IDbConnection conn;
        public readonly HttpClient _client = new HttpClient();
        private readonly IConfiguration _config;

        public RelatoriosRepository(ConnectionStrings connectionDb, IConfiguration config) : base(connectionDb)
        {
            conn = new SqlConnectionDB().ConnectionDB(_connectionDb.BD);
            _config = config;
        }
       
        public async Task<byte[]> RelatorioAluno(string token)
        {
            try
            {
                // DESCRIPTOGRAFA O TOKEN PARA USAR O ID NO REPORT SERVICE
                string id = Descriptografar(token);

                Aluno.CredenciaisReportService credenciais = new Aluno.CredenciaisReportService();

                HttpClientHandler Autenticacao = new HttpClientHandler();
                Autenticacao.Credentials = new NetworkCredential(credenciais.username, credenciais.password);

                using (HttpClient client = new HttpClient(Autenticacao))
                {
                    string servidor = "http://gw000552/";
                    string nomeRelatorio = "Report1";

                    string url = $"{servidor}ReportServer/Pages/ReportViewer.aspx?%2fSistemaRelatorioAluno%2f{nomeRelatorio}&rs:Format=PDF&cd_aluno={id}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] byteArray = await response.Content.ReadAsByteArrayAsync();

                        return byteArray;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Criptografar(string id)
        {
            string encryptionKey = "edeefrf15151511d";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aesAlg.IV = new byte[16]; 

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(id);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Descriptografar(string encryptedId)
        {
            string encryptionKey = "edeefrf15151511d";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aesAlg.IV = new byte[16]; 

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedId)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public async Task<string> VerificaAluno(string id)
        {
            using (SqlConnection conexao = new SqlConnection(conn.ConnectionString))
            {
                try
                {
                    var query = "SELECT cd_aluno FROM alunos WHERE cd_aluno = '" + id + "'";
                    conexao.Open();

                    SqlCommand comando = new SqlCommand(query, conexao);
                    SqlDataReader reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        string token = Criptografar(id);

                        return token;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    conexao.Close();
                }
            }
        }
    }
}
