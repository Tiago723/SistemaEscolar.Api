using Microsoft.Extensions.Logging;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Domain;
using SistemaEscolar.Api.Domain.Entidades;
using SistemaEscolar.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly ILogger<AutenticacaoService> _logger;
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        public AutenticacaoService(ILogger<AutenticacaoService> logger, IAutenticacaoRepository autenticacaoRepository)
        {
            _logger = logger;
            _autenticacaoRepository = autenticacaoRepository;
        }
        public Task<bool> ValidaLogin(string email, string senha, int nivel)
        {
            return _autenticacaoRepository.ValidaLogin(email, senha, nivel);
        }
    }
}
