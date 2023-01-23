using FluentValidation;
using JS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Commands
{
    public class RealizarPagamentoCommand : Command
    {
        public Guid idConta { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new RealizarPagamentoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RealizarPagamentoValidation : AbstractValidator<RealizarPagamentoCommand>
        {
            public RealizarPagamentoValidation()
            {
                RuleFor(c => c.idConta)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");
            }
        }
    }
}
