using Microsoft.AspNetCore.Mvc;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain.Entidades;
using System.Diagnostics;

namespace SistemaEscolar.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly ILogger<RelatoriosController> _logger;
        private readonly IRelatoriosService _relatoriosService;

        HttpClient Http = new HttpClient();

        public RelatoriosController(ILogger<RelatoriosController> logger, IRelatoriosService relatoriosService)
        {
            _logger = logger;
            _relatoriosService = relatoriosService;
        }

        [HttpGet]
        [Route("Aluno/{token}")]
        public async Task<IActionResult> RelatorioAluno(string id)
        {
            try
            {
                var PDF = await _relatoriosService.RelatorioAluno(id);

                if (PDF == null)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "RelatorioAluno" + " Erro no Serviço " + PDF, EventLogEntryType.Warning);
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

                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "RelatorioAluno" + " Sucesso " + PDF, EventLogEntryType.Information);

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
            var token = await _relatoriosService.GenerateToken(id);

            return token;
        }
    }
}