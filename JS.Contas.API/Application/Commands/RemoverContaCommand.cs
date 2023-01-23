using FluentValidation;
using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Commands
{
    public class RemoverContaCommand : Command
    {
        public Guid IdConta { get; set; }       

        public override bool EhValido()
        {
            ValidationResult = new RemoverContaValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RemoverContaValidation : AbstractValidator<RemoverContaCommand>
        {
            public RemoverContaValidation()
            {
                RuleFor(c => c.IdConta)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id da conta inválido");                 
            }
        }
    }
}
