using FluentValidation.Results;
using JS.Contas.API.Application.Commands;
using JS.Contas.API.Services;
using JS.Contas.Domain.Models;
using JS.Contas.Infra.Data;
using JS.Core.Mediator;
using JS.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Controllers
{
   // [Authorize]
    [Route("api/contas")]
    public class ContasController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly ContasServices _contaService;

        public ContasController(IMediatorHandler mediator, ContasServices contaService)
        {
            _mediator = mediator;
            _contaService = contaService;
        }

        [HttpGet()]
        public async Task<IActionResult> ObterContas()
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var contas = await _contaService.ObterContas();

            return CustomResponse(contas);
        }

        [HttpGet("Paginado")]
        public async Task<IActionResult> ObterContas([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var contas = await _contaService.ObterContas(ps, page, q);

            return CustomResponse(contas);
        }

        [HttpGet("Tipo/{tipo:int}")]
        public async Task<IActionResult> ObterContasPorTipo(int tipo)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _contaService.ObterContasPorTipo(tipo));
        }

        [HttpGet("Tipo/Paginado{tipo:int}")]
        public async Task<IActionResult> ObterContasPorTipo(int tipo, [FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _contaService.ObterContasPorTipo(tipo, ps, page, q));
        }

        [HttpGet("PorId/{id}")]
        public async Task<IActionResult> ObterContasPorId(Guid id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _contaService.ObterContasPorId(id));
        }

        [HttpGet("ObterItemsPorIdConta/{id}")]
        public async Task<IActionResult> ObterItemsPorIdConta(Guid id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _contaService.ObterItemsConta(id));
        }

        [HttpGet("Status/{tipo:int}")]
        public async Task<IActionResult> ObterContasPorStatus(int tipo ,  int status)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _contaService.ObterContasPorStatus(tipo, status));
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> AdicionarConta(AdicionarContaCommand conta)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _mediator.EnviarComando(conta));
        }       

        [HttpPost("Adicionar/Item/{idConta:guid}")]
        public async Task<IActionResult> AdicionarItemConta(AdicionarItemContaCommand itens, Guid idConta)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            return CustomResponse(await _mediator.EnviarComando(itens));

        }

        [HttpPut("Atualizar/{id:guid}")]
        public async Task<IActionResult> AtualizarConta(Guid id, AtualizarContaCommand conta)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);            

            if (id != conta.Id)
            {
                AdicionarErroProcessamento("O Id informado não é o mesmo informado na Query");
                return CustomResponse();
            }

            return CustomResponse(await _mediator.EnviarComando(conta));
        }

        [HttpDelete("Remover/Item/{idConta:guid}")]
        public async Task<IActionResult> RemoverItem(Guid idConta, RemoverItemContaCommand itensProdutos)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (idConta != itensProdutos.IdConta)
            {
                AdicionarErroProcessamento("O Id informado não é o mesmo informado na Query");
                return CustomResponse();
            }

            return CustomResponse(await _mediator.EnviarComando(itensProdutos));
        }

        [HttpDelete("Remover/{idConta:guid}")]
        public async Task<IActionResult> RemoverConta(RemoverContaCommand conta)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
           

            return CustomResponse(await _mediator.EnviarComando(conta));
        }

        [HttpPut("Realizar-Pagamento")]
        public async Task<IActionResult> RealizarPagamentoConta(RealizarPagamentoCommand conta)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);           

            return CustomResponse(await _mediator.EnviarComando(conta));
        }
    }
}
