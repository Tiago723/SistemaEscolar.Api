using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain.Entidades;
using System.Diagnostics;
using System.Net;

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
        [Route("GeraTokenAluno")]
        public async Task<IActionResult> GeraTokenAluno(string id)
        {
            var token = await _relatoriosService.VerificaAluno(id);

            if (token != null)
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "GeraTokenAluno" + " Sucesso " + token, EventLogEntryType.Information);
                return Ok(token);
            }
            else
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "GeraTokenAluno" + "Aluno não cadastrado" + token, EventLogEntryType.Warning);
                return StatusCode(StatusCodes.Status404NotFound, "Aluno não cadastrado");
            }
        }

        [HttpGet]
        [Route("Aluno/")]
        public async Task<IActionResult> RelatorioAluno(string token)
        {
            var RetornoPDF = await _relatoriosService.RelatorioAluno(token);

            if (RetornoPDF == null)
            {
                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "RelatorioAluno" + "Erro no serviço" + RetornoPDF, EventLogEntryType.Warning);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no serviço");
            }
            else
            {
                //Obtém a data e hora atual
                DateTime dateTime = DateTime.Now;

                // Obtém somente a data
                string dt = dateTime.ToShortDateString();

                // Atribui a data como parte do nome do arquivo para download
                string ArquivoPDF = "RelatórioAluno_" + dt.ToString() + ".pdf";

                // Configura o cabeçalho de resposta para abrir o PDF no navegador
                Response.Headers.Add("Content-Disposition", $"inline; filename=\"{ArquivoPDF}\"");

                string tipoConteudo = "application/pdf";

                EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "RelatorioAluno" + " Sucesso " + RetornoPDF, EventLogEntryType.Information);
                return File(RetornoPDF, tipoConteudo);
            }
        }
    }
}