using JS.Contas.Domain.Models;
using System;

namespace JS.Contas.API.Application.DTO
{
    public class ItensContasDTO
    {
        public ItensContasDTO(Guid idItem, Guid contaId, Guid produtoId, string nome, int quantidade, decimal valor)
        {
            IdItem = idItem;
            ContaId = contaId;
            ProdutoId = produtoId;
            ProdutoNome = nome;
            ValorUnitario = valor;
            Quantidade = quantidade;
        }

        public ItensContasDTO() { }

        public Guid IdItem { get; set; }
        public Guid ContaId { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }

        public static ContaItem ParaContaItem(ItensContasDTO itensContasDTO)
        {
            return new ContaItem(itensContasDTO.ContaId, itensContasDTO.ProdutoId, itensContasDTO.ProdutoNome, itensContasDTO.Quantidade,
                itensContasDTO.ValorUnitario);
        }

        public static ItensContasDTO ParaContaItemDTO(ContaItem itensContas)
        {
            return new ItensContasDTO(itensContas.Id, itensContas.ContaId, itensContas.ProdutoId, itensContas.ProdutoNome, itensContas.Quantidade,
                itensContas.ValorUnitario);
        }
    }
}
