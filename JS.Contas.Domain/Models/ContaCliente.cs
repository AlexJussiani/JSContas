using FluentValidation.Results;
using JS.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JS.Contas.Domain.Models
{
    public class ContaCliente : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataCompra { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public TipoConta TipoConta { get; private set; }
        public StatusConta StatusConta { get; private set; }
        public bool Excluido { get; private set; }

        public List<ContaItem> ContaItems { get; set; } = new List<ContaItem>();

        /* public readonly List<ContaItem>  _itensConta;*/// { get; private set; } = new List<ContaItem>();

        //public IReadOnlyCollection<ContaItem> ContaItems => _itensConta;

        protected ContaCliente() { }

        public ContaCliente(Guid clienteId, decimal valorTotal, List<ContaItem> pedidoItems,
           DateTime dataVencimento, DateTime dataCompra, int tipoConta)
        {
            ClienteId = clienteId;
            ValorTotal = valorTotal;
            ContaItems = pedidoItems;
            DataVencimento = dataVencimento;
            DataCompra = dataCompra;
            TipoConta = (tipoConta == 1 ? TipoConta.Pagar : TipoConta.Receber);
        }
       

        public void setDataVencimento(DateTime data)
        {
            DataVencimento = data;
        }

        public void setDataCompra(DateTime data)
        {
            DataCompra = data;
        }

        public void setCliente(Guid id)
        {
            ClienteId = id;
        }

        public void setContasItens(List<ContaItem> pedidoItems)
        {
            ContaItems = pedidoItems;
        }          

        public void CalcularValorConta()
        {
            ValorTotal = ContaItems.Sum(p => p.CalcularValor());
        }

        public void ContaPendente()
        {
            StatusConta = StatusConta.Pendente;
        }

        public void PagarConta()
        {
            StatusConta = StatusConta.Pago;
        }
        public void AtrasarConta()
        {
            StatusConta = StatusConta.Atrasado;
        }

        public void CancelarConta()
        {
            StatusConta = StatusConta.Cancelado;
        }
        

        public void AdicionarItem(ContaItem item)
        {
            item.AssociarConta(Id);

            if (ContaItemExistente(item))
            {
                var itemExistente = ObterPorProdutoId(item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;
                ContaItems.Remove(itemExistente);
            }
            ContaItems.Add(item);
            CalcularValorConta();
        }

        public ContaItem ObterPorProdutoId(Guid produtoId)
        {
            return ContaItems.FirstOrDefault(p => p.ProdutoId == produtoId);
        }

        public void RemoverConta()
        {
            Excluido = true;
        }

        public bool ContaItemExistente(ContaItem contaItem)
        {
            return ContaItems.Any(p => p.ProdutoId == contaItem.ProdutoId);
        }

        public void RemoverItem(ContaItem item)
        {
            ContaItems.Remove(ObterPorProdutoId(item.ProdutoId));
            CalcularValorConta();
        }
       
    }
}


