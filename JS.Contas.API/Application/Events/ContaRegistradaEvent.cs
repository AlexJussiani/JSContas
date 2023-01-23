using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Events
{
    public class ContaRegistradaEvent : Event
    {
        public Guid ContaId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataVencimento { get; private set; }

        public int TipoConta { get; set; }

        public int StatusConta { get; set; }
        public List<ItensContasDTO> ContaItems { get; set; }

        public ContaRegistradaEvent(
            Guid contaId,
            Guid clienteId, 
            decimal valorTotal, 
            DateTime dataVencimento, 
            int tipoConta, 
            int statusConta,
            List<ItensContasDTO> contaItems)
        {
            ContaId = contaId;
            AggregateId = contaId;
            ClienteId = clienteId;
            ValorTotal = valorTotal;
            DataVencimento = dataVencimento;
            TipoConta = tipoConta;
            StatusConta = statusConta;
            ContaItems = contaItems;
        }
    }
}
