using Elmah.Io.Client;
using FluentValidation.Results;
using JS.Contas.API.Application.DTO;
using JS.Contas.API.Application.Events;
using JS.Contas.Domain.Models;
using JS.Contas.Infra.Data.Repository;
using JS.Core.Mediator;
using JS.Core.Messages;
using JS.Core.Messages.Integration;
using JS.MessageBus;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Commands
{
    public class ContaCommandHandler : CommandHandler,
        IRequestHandler<AdicionarContaCommand, ValidationResult>,
        IRequestHandler<AtualizarContaCommand, ValidationResult>,
        IRequestHandler<AdicionarItemContaCommand, ValidationResult>,
        IRequestHandler<RemoverItemContaCommand, ValidationResult>,
        IRequestHandler<RealizarPagamentoCommand, ValidationResult>,
        IRequestHandler<RemoverContaCommand, ValidationResult>
    {
        private readonly IMessageBus _bus;
        private readonly IContasRepository _contasRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ContaCommandHandler(IMessageBus bus, IContasRepository contasRepository, IMediatorHandler mediatorHandler)
        {
            _bus = bus;
            _contasRepository = contasRepository;
            _mediatorHandler = mediatorHandler;
        }

        //Adicionar conta
        public async Task<ValidationResult> Handle(AdicionarContaCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            //MapearConta
            var conta = MapearConta(message);

            //Status conta pendente de pagamento/recebimento
            conta.ContaPendente();

            //Calcular conta
            conta.CalcularValorConta();           

            _contasRepository.AdicionarConta(conta);

            //Lançar Evento
            //conta.AdicionarEvento(
            //    new ContaRegistradaEvent(
            //        conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList())
            //    );

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        //Atualizar Conta
        public async Task<ValidationResult> Handle(AtualizarContaCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var conta = await _contasRepository.ObterPorId(message.Id);

            if (conta == null)
            {
                AdicionarErro("Conta não localizada");
                return ValidationResult;
            }          

            conta.setDataVencimento(message.DataVencimento);
            conta.setDataCompra(message.DataCompra);
            conta.setCliente(message.ClienteId);

            while (conta.ContaItems.Count != 0)
            {
                var itemConta = conta.ObterPorProdutoId(conta.ContaItems[0].ProdutoId);
                conta.RemoverItem(itemConta);
                _contasRepository.RemoverItemConta(itemConta);
            }

            foreach (var item in message.ContaItems.Select(ItensContasDTO.ParaContaItem).ToList())           
                conta = AtualizarItem(conta, item);                

            //calcular o valor da conta
            conta.CalcularValorConta();

            _contasRepository.AtualizarConta(conta);

            //Lançar Evento
            //conta.AdicionarEvento(
            //    new ContaAtualizadaEvent(
            //         conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList()));

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        //Adicionar Item Conta
        public async Task<ValidationResult> Handle(AdicionarItemContaCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var conta = await _contasRepository.ObterPorId(message.IdConta);

            if (conta == null)
            {
                AdicionarErro("Conta não localizada");
                return ValidationResult;
            }

            foreach (var item in message.ContaItems.Select(ItensContasDTO.ParaContaItem).ToList())           
                conta = AtualizarItem(conta, item);            

            _contasRepository.AtualizarConta(conta);

            //conta.AdicionarEvento(
            //     new ContaAtualizadaEvent(
            //         conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList()));

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        //Remover Item Conta
        public async Task<ValidationResult> Handle(RemoverItemContaCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var conta = await _contasRepository.ObterPorId(message.IdConta);

            if (conta == null)
            {
                AdicionarErro("Conta não localizada");
                return ValidationResult;
            }

            foreach (var item in message.Produtos)
            {
                var itemConta = conta.ObterPorProdutoId(item);
                if(itemConta == null)
                {
                    AdicionarErro("Item não localizado para essa conta");
                    return ValidationResult;
                }
                conta.RemoverItem(itemConta);
                _contasRepository.RemoverItemConta(itemConta);
            }

            _contasRepository.AtualizarConta(conta);

            //conta.AdicionarEvento(
            //     new ContaAtualizadaEvent(
            //         conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList()));

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        //Realizar Pagamento
        public async Task<ValidationResult> Handle(RealizarPagamentoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var conta = await _contasRepository.ObterPorId(message.idConta);

            if (conta == null)
            {
                AdicionarErro("Conta não localizada");
                return ValidationResult;
            }

            conta.PagarConta();

            _contasRepository.AtualizarConta(conta);

            //adicionar evento para event store
            //conta.AdicionarEvento(
            //     new ContaAtualizadaEvent(
            //         conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList()));

            //envia uma mensagem de pagamento realizado            
            await _bus.PublishAsync(new AdicionarMovimentacaoFinanceiraIntegrationEvent(
                conta.Id,
                conta.Codigo,
                conta.ValorTotal,
                conta.DataCadastro,
                DateTime.Now,
                (int)conta.TipoConta));

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        //Remover Conta
        public async Task<ValidationResult> Handle(RemoverContaCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var conta = await _contasRepository.ObterPorId(message.IdConta);

            if (conta == null)
            {
                AdicionarErro("Conta não localizada");
                return ValidationResult;
            }

            _contasRepository.RemoverConta(conta);          

            //await _mediatorHandler.PublicarEvento(new ContaRemovidaEvent(
            //         conta.Id,
            //        conta.ClienteId,
            //        conta.ValorTotal,
            //        conta.DataVencimento,
            //        ((int)conta.TipoConta),
            //        ((int)conta.StatusConta),
            //        conta.ContaItems.Select(ItensContasDTO.ParaContaItemDTO).ToList()));

            if(conta.StatusConta == StatusConta.Pago)
            {
                await _bus.PublishAsync(new DeletarMovimentacaoFinanciraaIntegrationEvent(conta.Id));
            }

            return await PersistirDados(_contasRepository.UnitOfWork);
        }

        private ContaCliente MapearConta(AdicionarContaCommand message)
        {
            var conta = new ContaCliente(message.ClienteId, 
                                         message.ValorTotal,
                                         message.ContaItems.Select(ItensContasDTO.ParaContaItem).ToList(),
                                         message.DataVencimento,
                                         message.DataCompra,
                                         message.TipoConta);

            return conta;
        }

        private ContaCliente AtualizarItem(ContaCliente conta, ContaItem item)
        {            
                var produtoItemExistente = conta.ContaItemExistente(item);
                conta.AdicionarItem(item);

                if (produtoItemExistente)
                {
                    _contasRepository.AtualizarItem(conta.ObterPorProdutoId(item.ProdutoId));
                }
                else
                {
                    _contasRepository.AdicionarItem(item);
                }           

            return conta;
        }

    }
}
