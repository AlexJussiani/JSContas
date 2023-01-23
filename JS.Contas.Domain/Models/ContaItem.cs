using JS.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JS.Contas.Domain.Models
{
    public class ContaItem : Entity
    {       
        public Guid ContaId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public bool Excluido { get; private set; }

        // EF Relation
        [JsonIgnore]
        public ContaCliente Conta { get; private set; }

        public ContaItem() { }

        public ContaItem(Guid contaId, Guid produtoId, string produtoNome, int quantidade,
            decimal valorUnitario)
        {
            ContaId = contaId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public void setQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }

        public void setProdutoNome(string produtoNome)
        {
            ProdutoNome = produtoNome;
        }

        internal void AssociarConta(Guid contaId)
        {
            ContaId = contaId;
        }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade = unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
