using Microsoft.AspNetCore.Mvc;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain.Entidades;
using System.Diagnostics;

namespace SistemaEscolar.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly ILogger<AlunoController> _logger;
        private readonly IAlunoService _alunoService;

        HttpClient Http = new HttpClient();

        public AlunoController(ILogger<AlunoController> logger, IAlunoService alunoService)
        {
            _logger = logger;
            _alunoService = alunoService;
        }

        [HttpPost("CadastraAluno")]
        public async Task<IActionResult> CadastraAluno(Aluno.CadastraAluno parametros)
        {
            var Retorno = await _alunoService.CadastraAluno(parametros);

            if (Retorno == null)
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "CadastraAluno" + " Erro no Serviço " + Retorno, EventLogEntryType.Warning);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Serviço");
            }
            else
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "CadastraAluno" + " Sucesso " + Retorno, EventLogEntryType.Information);
                return Ok(Retorno);
            }
        }

        [HttpGet("ConsultaAluno")]
        public async Task<IActionResult> ConsultaAluno(int Id)
        {
            var Retorno = await _alunoService.ConsultaAluno(Id);

            if (Retorno == null)
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "ConsultaAluno" + " Erro no Serviço " + Retorno, EventLogEntryType.Warning);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Serviço");
            }
            else
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "ConsultaAluno" + " Sucesso " + Retorno, EventLogEntryType.Information);
                return Ok(Retorno);
            }
        }

        [HttpGet("ListaAlunos")]
        public async Task<IActionResult> ListaAlunos()
        {
            var Retorno = await _alunoService.ListaAlunos();

            if (Retorno == null)
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "ListaAlunos" + " Erro no Serviço " + Retorno, EventLogEntryType.Warning);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Serviço");
            }
            else
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "ListaAlunos" + " Sucesso " + Retorno, EventLogEntryType.Information);
                return Ok(Retorno);
            }
        }

        [HttpPut("EditaAluno")]
        public async Task<IActionResult> EditaAluno(Aluno.EditaAluno parametros)
        {
            var Retorno = await _alunoService.EditaAluno(parametros);

            if (Retorno != null)
            {
                return Ok(Retorno);
            }
            else
            {
                return null;
            }
        }

        [HttpGet("ExcluiAluno")]
        public async Task<IActionResult> ExcluiAluno(int Id)
        {
            var Retorno = await _alunoService.ExcluiAluno(Id);

            if (Retorno != null)
            {
                return Ok(Retorno);
            }
            else
            {
                return null;
            }
        }

        [HttpGet("ValidaLogin")]
        public async Task<IActionResult> ValidaLogin(string email, string senha, int nivel)
        {
            var Retorno = await _alunoService.ValidaLogin(email, senha, nivel);

            try
            {
                if (Retorno == true)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "ValidaLogin" + " Sucesso " + Retorno, EventLogEntryType.Information);
                    return Ok(Retorno);
                }
                else if (Retorno == false)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status401Unauthorized) + "ValidaLogin" + " Não autorizado " + Retorno, EventLogEntryType.Warning);
                    return StatusCode(StatusCodes.Status401Unauthorized, "Usuário ou senha inválidos");
                }
                else
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "ValidaLogin" + " Erro no Serviço " + Retorno, EventLogEntryType.Warning);
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Serviço");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Relatorio/{token}")]
        public async Task<IActionResult> RelatorioPDF(string id)
        {
            try
            {
                var PDF = await _alunoService.RelatorioPDF(id);

                if (PDF == null)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "Relatorio" + " Erro no Serviço " + PDF, EventLogEntryType.Warning);
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Serviço");
                }
                else
                {
                    //Obtém a data e hora atual
                    DateTime dateTime = DateTime.Now;

                    // Obtém somente a data
                    string dt = dateTime.ToShortDateString();

                    // Atribui a data como parte do nome do arquivo para download
                    string ArquivoPDF = "Relatório_" + dt.ToString() + ".pdf";

                    // Configura o cabeçalho de resposta para abrir o PDF no navegador
                    Response.Headers.Add("Content-Disposition", $"inline; filename=\"{ArquivoPDF}\"");

                    string tipoConteudo = "application/pdf";

                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "Relatorio" + " Sucesso " + PDF, EventLogEntryType.Information);

                    return File(PDF, tipoConteudo);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro no servidor");
            }
        }

        [HttpGet]
        [Route("Token")]
        public async Task<string> GenerateToken(string id)
        {
            var token = await _alunoService.GenerateToken(id);

            return token;
        }
    }
}