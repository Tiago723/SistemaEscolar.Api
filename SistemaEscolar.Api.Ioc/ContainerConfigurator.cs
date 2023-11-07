using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaEscolar.Api.Application.Interfaces;
using SistemaEscolar.Api.Application.Services;
using SistemaEscolar.Api.Domain.Interfaces;
using SistemaEscolar.Api.Infra.Connection;
using SistemaEscolar.Api.Infra.Repositories;

namespace SistemaEscolar.Api.Ioc
{
    public static class ContainerConfigurator
    {
        public static void AddDependencias(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<ConnectionStrings>();

            //Injeção de dependência dos repositórios
            serviceCollection.AddSingleton<IAlunoRepository, AlunoRepository>();
            serviceCollection.AddSingleton<IAutenticacaoRepository, AutenticacaoRepository>();

            //Injeção de dependência dos serviços
            serviceCollection.AddSingleton<IAlunoService, AlunoService>();
            serviceCollection.AddSingleton<IAutenticacaoService, AutenticacaoService>();

            serviceCollection.AddHealthCheckUri(configuration);
        }
    }
}