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
    public class RelatoriosService : IRelatoriosService
    {
        private readonly ILogger<RelatoriosService> _logger;
        private readonly IRelatoriosRepository _relatoriosRepository;
        public RelatoriosService(ILogger<RelatoriosService> logger, IRelatoriosRepository relatoriosRepository)
        {
            _logger = logger;
            _relatoriosRepository = relatoriosRepository;
        }
        public Task<byte[]> RelatorioAluno(string id)
        {
            return _relatoriosRepository.RelatorioAluno(id);
        }
        public Task<string> GenerateToken(string id)
        {
            return _relatoriosRepository.GenerateToken(id);
        }
    }
}
