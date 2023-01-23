using FluentValidation;
using JS.Contas.API.Application.DTO;
using JS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JS.Contas.API.Application.Commands
{
    public class RemoverItemContaCommand : Command
    {
        public Guid IdConta { get; set; }       
        public List<Guid> Produtos { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new RemoverItemContaValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RemoverItemContaValidation : AbstractValidator<RemoverItemContaCommand>
        {
            public RemoverItemContaValidation()
            {
                RuleFor(c => c.IdConta)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id da conta inválido");               

                RuleFor(c => c.Produtos.Count)
                    .GreaterThan(0)
                    .WithMessage("Nenhum produto informado");                   
            }
        }
    }
}
