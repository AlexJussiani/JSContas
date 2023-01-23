using JS.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Contas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using JS.Core.Mediator;

namespace JS.Contas.Infra.Data.Repository
{
    public class ContaRepository : IContasRepository
    {
        private readonly ContasContext _context;

        public ContaRepository(ContasContext context, IMediatorHandler mediatorHandler)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;      

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<ContaCliente>> ObterContas()
        {
            return await _context.Conta.AsNoTracking()
                .Where(c => c.Excluido == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContaCliente>> ObterContas(int pageSize, int pageIndex, string query = null)
        {
            return await _context.Conta
                .AsNoTracking()
                .Where(c => c.Excluido == false)
                .Skip(pageSize * (pageIndex))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContaCliente>> ObterContasPorTipo(int tipo, int pageSize, int pageIndex, string query = null)
        {
            return await _context.Conta
                .AsNoTracking()
                .Where(c => c.Excluido == false)
                .Where(c => ((int)c.TipoConta) == tipo)
                .Skip(pageSize * (pageIndex))
                .Take(pageSize).
                ToListAsync();
        }

        public async Task<IEnumerable<ContaCliente>> ObterContasPorTipo(int tipo)
        {
            return await _context.Conta
                .AsNoTracking()
                .Where(c => ((int)c.TipoConta) == tipo && c.Excluido == false)
                .ToListAsync();
        }

        public Task<IEnumerable<ContaCliente>> ObterPorCliente(TipoConta tipo, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ContaCliente> ObterPorId(Guid id)
        {
            return await _context.Conta
                .Where(c => c.Excluido == false)
                .Include(c => c.ContaItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ContaItem>> ObterItemPorIdConta(Guid id)
        {
            return await _context.ContaItens.Where(i => i.ContaId.Equals(id)).ToListAsync();
        }

        public async Task<IEnumerable<ContaCliente>> ObterPorId1(Guid id)
        {
            return await _context.Conta
                .Where(c => c.Excluido == false)
                .Include(c => c.ContaItems)
                .AsNoTracking()
                .Where(c => c.Id == id).ToListAsync();
        }

        public async Task<ContaItem> ObterItemPorId(Guid id)
        {
            return await _context.ContaItens.AsNoTracking()
                .Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<ContaCliente>> ObterPorNome(TipoConta tipo, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContaCliente>> ObterPorStatus(int tipo, int status)
        {
            return await _context.Conta.AsNoTracking()
                .Where(c => ((int)c.TipoConta) == tipo && ((int) c.StatusConta == status) && c.Excluido == false)
                .ToListAsync();
        }
        public async Task<int> TotalContas()
        {
            return await _context.Conta
                .AsNoTracking()
                .Where(c => c.Excluido == false)
                .CountAsync();
        }

        public async Task<int> TotalContas(int tipo)
        {
            return await _context.Conta
                .AsNoTracking().Where(c => ((int)c.TipoConta) == tipo && c.Excluido == false)
                .CountAsync();
        }

        public void AdicionarConta(ContaCliente conta)
        {
            _context.Conta.Add(conta);
        }

        public void AdicionarItem(ContaItem item)
        {
            _context.ContaItens.Add(item);
        }

        public void AtualizarConta(ContaCliente conta)
        {
            _context.Conta.Update(conta);
        }
        public void AtualizarItem(ContaItem item)
        {
            _context.ContaItens.Update(item);
        }

        public void RemoverItemConta(ContaItem contaItem)
        {
            _context.ContaItens.Remove(contaItem);
        }

        public void RemoverConta(ContaCliente conta)
        {
            _context.Conta.Remove(conta);
        }       
    }
}