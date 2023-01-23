using FluentValidation;
using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Commands
{
    public class AdicionarContaCommand : Command
    {
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataCompra { get; set; }
        public int TipoConta { get; set; }
        public int StatusConta { get; set; }
        public List<ItensContasDTO> ContaItems { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarContaValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AdicionarContaValidation : AbstractValidator<AdicionarContaCommand>
        {
            public AdicionarContaValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.DataVencimento)
                    .NotNull()
                    .WithMessage("Data de vencimento deve ser informada");

                RuleFor(c => c.ContaItems.Count)
                    .GreaterThan(0)
                    .WithMessage("O pedido precisa ter no mínimo 1 item");

                RuleFor(c => c.StatusConta)
            .NotNull()
            .WithMessage("O pedido precisa ter no mínimo 1 item");
            }
        }
    }  
}