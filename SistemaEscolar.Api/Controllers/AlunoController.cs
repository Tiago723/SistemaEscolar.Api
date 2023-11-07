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
    }
}