using FluentValidation;
using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;

namespace JS.Contas.API.Application.Commands
{
    public class AdicionarItemContaCommand : Command
    {
        public Guid IdConta { get; set; }
        public List<ItensContasDTO> ContaItems { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarItemContaValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AdicionarItemContaValidation : AbstractValidator<AdicionarItemContaCommand>
        {
            public AdicionarItemContaValidation()
            {
                RuleFor(c => c.IdConta)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id da conta inválido");

                RuleFor(c => c.ContaItems.Count)
                     .GreaterThan(0)
                     .WithMessage("O pedido precisa ter no mínimo 1 item");
            }
        }
    }
}
