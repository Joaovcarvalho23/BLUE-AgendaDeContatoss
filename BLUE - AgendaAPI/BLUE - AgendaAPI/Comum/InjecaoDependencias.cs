using Agenda.Application.Services;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure.Data;
using Agenda.Infrastructure.Repositorios;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace Agenda.API.Comum
{
    public static class InjecaoDependencia
    {
        public static IServiceCollection AdicionarInjecoes(this IServiceCollection services, IConfiguration configuration)
        {
            // Db Context
            services.AddDbContext<AgendaContext>(options =>
            {
                var conexao = configuration["STR_CONEXAO"];

                if (string.IsNullOrEmpty(conexao))
                    throw new InvalidOperationException("A variável de ambiente STR_CONEXAO não está definida.");

                options.UseSqlServer(
                    conexao,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                );
            });

            //Se quiser utilizar a connection string do seu banco no appsettings.json, utilizar metodo abaixo:
            #region Metodo de conexao com connection string no appsettings.json
            // Db Context
            //services.AddDbContext<AgendaContext>(options => 
            //    options.UseSqlServer(configuration.GetConnectionString("strConexao"),
            //        sqlOptions => sqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 5,
            //            maxRetryDelay: TimeSpan.FromSeconds(10),
            //            errorNumbersToAdd: null
            //         )
            //    )
            // );
            #endregion

            // DI
            services.AddScoped<IContatoRepositorio, ContatoRepositorio>();
            services.AddScoped<ContatoService>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<UsuarioService>();

            services.AddControllers();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Agenda.Application.Validations.Usuarios.ValidadorRegistroUsuarioDto>();

            return services;
        }
    }
}