using Microsoft.AspNetCore.Mvc;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain.Entidades;
using System.Diagnostics;

namespace SistemaEscolar.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly IAutenticacaoService _autenticacaoService;

        HttpClient Http = new HttpClient();

        public AutenticacaoController(ILogger<AutenticacaoController> logger, IAutenticacaoService autenticacaoService)
        {
            _logger = logger;
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("ValidaLogin")]
        public async Task<IActionResult> ValidaLogin(string email, string senha, int nivel)
        {
            var Retorno = await _autenticacaoService.ValidaLogin(email, senha, nivel);

            try
            {
                if (Retorno == true)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status200OK) + "ValidaLogin" + " Sucesso " + Retorno, EventLogEntryType.Information);
                    return Ok(Retorno);
                }
                else if (Retorno == false)
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status401Unauthorized) + "ValidaLogin" + " N�o autorizado " + Retorno, EventLogEntryType.Warning);
                    return StatusCode(StatusCodes.Status401Unauthorized, "Usu�rio ou senha inv�lidos");
                }
                else
                {
                    EventLog.WriteEntry("SistemaEscolar.Api", Convert.ToString(StatusCodes.Status503ServiceUnavailable) + "ValidaLogin" + " Erro no Servi�o " + Retorno, EventLogEntryType.Warning);
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro no Servi�o");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}