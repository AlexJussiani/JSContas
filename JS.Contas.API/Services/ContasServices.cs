using FluentValidation.Results;
using JS.Contas.Domain.Models;
using JS.Contas.Infra.Data.Repository;
using JS.Core.Messages;
using JS.Core.Messages.Integration;
using JS.MessageBus;
using JS.WebAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Services
{   
    public class ContasServices : CommandHandler
    {
        private readonly IContasRepository _contaRepository;
        private readonly IMessageBus _bus;

        public ContasServices(IContasRepository contaRepository, IMessageBus bus)
        {
            _contaRepository = contaRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> CadastrarConta(ContaCliente conta)
        {
            conta.CalcularValorConta();
            _contaRepository.AdicionarConta(conta);

            return await PersistirDados(_contaRepository.UnitOfWork);            
        }

        public async Task<IEnumerable<ContaCliente>> ObterContas()
        {
            var contas = await _contaRepository.ObterContas();
            return  contas;
        }
        

        public async Task<IEnumerable<ContaCliente>> ObterContasPorTipo(int tipo)
        {
            var contas = await _contaRepository.ObterContasPorTipo(tipo);
            return contas;
        }

        public async Task<PagedResult<ContaCliente>> ObterContasPorTipo(int tipo, int pageSize, int pageIndex, string query = null)
        {
            var contas = await _contaRepository.ObterContasPorTipo(tipo, pageSize, pageIndex, query);

            return new PagedResult<ContaCliente>()
            {
                List = contas,
                TotalResults = await _contaRepository.TotalContas(tipo),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }

        public async Task<IEnumerable<ContaItem>> ObterItemsConta(Guid idConta)
        {
            var itemsConta = await _contaRepository.ObterItemPorIdConta(idConta);
            return itemsConta;
        }

        public async Task<ContaCliente> ObterContasPorId(Guid id)
        {
            var contas = await _contaRepository.ObterPorId(id);
            return contas;
        }

        public async Task<IEnumerable<ContaCliente>> ObterContasPorStatus(int tipo, int status)
        {
            var contas = await _contaRepository.ObterPorStatus(tipo, status);
            return contas;
        }

        public async Task<PagedResult<ContaCliente>> ObterContas(int pageSize, int pageIndex, string query = null)
        {
            var contasPagar = await _contaRepository.ObterContas(pageSize, pageIndex, query);

            return new PagedResult<ContaCliente>()
            {
                List = contasPagar,
                TotalResults = await _contaRepository.TotalContas(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }

        //public async Task<ResponseMessage> RegistrarPagamentoRecebimento(Guid idConta)
        //{
        //    var conta = await _contaRepository.ObterPorId(idConta);
        //    conta.StatusConta = StatusConta.Pago;
        //    var movimentacaoRegistrada = new MovimentacaoFinanceiraIntegrationEvent(
        //        Guid.NewGuid(), conta.Codigo, conta.Id, conta.ValorTotal, DateTime.Now, conta.DataVencimento, ((int)conta.TipoConta));
        //    try
        //    {
        //        //Comunicação com a API Movimentações através do RabbitMQ
        //        _contaRepository.UpdateConta(conta);
        //        await PersistirDados(_contaRepository.UnitOfWork);
        //        return await _bus.RequestAsync<MovimentacaoFinanceiraIntegrationEvent, ResponseMessage>(movimentacaoRegistrada);

        //    }
        //    catch
        //    {
        //        conta.StatusConta = StatusConta.Pendente;
        //        _contaRepository.UpdateConta(conta);
        //        await PersistirDados(_contaRepository.UnitOfWork);
        //        throw;
        //    }
        //}

        public async Task<ResponseMessage> DeletarConta(Guid idConta)
        {
            var conta = await _contaRepository.ObterPorId(idConta);
            if(conta.StatusConta == StatusConta.Pago)
            {
                //comunicar com a API Movimentações através do RabbitMQ
            }
            return null;
        }
    }
}
