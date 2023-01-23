using Microsoft.Extensions.DependencyInjection;
using JS.Contas.Infra.Data.Repository;
using JS.Contas.Infra.Data;
using JS.Core.Mediator;
using JS.Contas.API.Services;
using JS.Contas.API.Application.Commands;
using FluentValidation.Results;
using MediatR;
using JS.EventSourcing;
using JS.Core.Data.EventSourcing;
using JS.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Http;

namespace JS.Contas.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            //Command
            services.AddScoped<IRequestHandler<AdicionarContaCommand, ValidationResult>, ContaCommandHandler>();

            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Event Sourcing
            services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

            services.AddScoped<IContasRepository, ContaRepository>();
            services.AddScoped<ContasContext>();
            services.AddScoped<ContasServices>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}