using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Contas.Domain.Models;
using JS.Core.Data;

namespace JS.Contas.Infra.Data.Repository
{
    public interface IContasRepository : IRepository<ContaCliente>
    {
        //conta
        Task<int> TotalContas();
        Task<int> TotalContas(int tipo);
        Task<IEnumerable<ContaCliente>> ObterPorNome(TipoConta tipo, string name);
        Task<ContaCliente> ObterPorId(Guid id);
        Task<IEnumerable<ContaCliente>> ObterPorCliente(TipoConta tipo, Guid id);
        Task<IEnumerable<ContaCliente>> ObterContasPorTipo(int tipo);
        Task<IEnumerable<ContaCliente>> ObterContasPorTipo(int tipo, int pageSize, int pageIndex, string query = null);
        Task<IEnumerable<ContaCliente>> ObterContas();
        Task<IEnumerable<ContaCliente>> ObterContas(int pageSize, int pageIndex, string query = null);
        Task<IEnumerable<ContaCliente>> ObterPorStatus(int tipo, int status);
        void AtualizarConta(ContaCliente conta);
        void AdicionarConta(ContaCliente conta);
        void RemoverConta(ContaCliente conta);

        //item conta
        Task<ContaItem> ObterItemPorId(Guid id);
        Task<IEnumerable<ContaItem>> ObterItemPorIdConta(Guid id);
        void AdicionarItem(ContaItem item);
        void AtualizarItem(ContaItem item);
        void RemoverItemConta(ContaItem contaItem);

    }
}