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
        public async Task<byte[]> RelatorioAluno(string Id)
        {
            try
            {
                Aluno.CredenciaisReportService credenciais = new Aluno.CredenciaisReportService();

                HttpClientHandler Autenticacao = new HttpClientHandler();
                Autenticacao.Credentials = new NetworkCredential(credenciais.username, credenciais.password);

                using (HttpClient client = new HttpClient(Autenticacao))
                {
                    string servidor = "http://localhost/";
                    string nomeRelatorio = "Report1";

                    string url = $"{servidor}ReportServer/Pages/ReportViewer.aspx?%2fRelatorio1%2f{nomeRelatorio}&rs:Format=PDF&id={Id}";

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
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GenerateToken(string id)
        {
            // Gere uma chave secreta de 32 bytes (256 bits)
            string chaveSecreta = GenerateRandomKey(32);

            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(chaveSecreta)))
            {
                // Converte o ID em bytes
                byte[] idBytes = Encoding.UTF8.GetBytes(id);

                // ComputeHash irá gerar um hash do ID
                byte[] hashBytes = hmac.ComputeHash(idBytes);

                // Converte o hash em uma representação de string segura
                string autentication = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return autentication;
            }
        }

        public string GenerateRandomKey(int length)
        {
            using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
            {
                // Cria um array de bytes com o comprimento especificado pela variável 'length'
                byte[] keyBytes = new byte[length];

                // Gera bytes seguros usando o RNGCryptoServiceProvider e armazena no array keyBytes
                rngCrypto.GetBytes(keyBytes);

                // Converte os bytes aleatórios em string hexadecimal e remove os hifens e gera uma string de chave aleatória hexadecimal
                return BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
            }
        }
    }
}
