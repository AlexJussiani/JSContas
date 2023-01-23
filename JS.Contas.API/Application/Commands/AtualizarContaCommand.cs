using FluentValidation;
using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;

namespace JS.Contas.API.Application.Commands
{
    public class AtualizarContaCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataCompra { get; set; }
        public List<ItensContasDTO> ContaItems { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AtualizarContaValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AtualizarContaValidation : AbstractValidator<AtualizarContaCommand>
        {
            public AtualizarContaValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.DataVencimento)
                    .NotNull()
                    .WithMessage("Data de vencimento deve ser informada");
            }
        }
    }
}
