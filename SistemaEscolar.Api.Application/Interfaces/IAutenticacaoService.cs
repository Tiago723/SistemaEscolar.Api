using SistemaEscolar.Api.Domain;
using SistemaEscolar.Api.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEscolar.Api.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<bool> ValidaLogin(string email, string senha, int nivel);
    }
}
