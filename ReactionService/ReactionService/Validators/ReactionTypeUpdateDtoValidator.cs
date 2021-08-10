using FluentValidation;
using ReactionService.Models.DTOs.ReactionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Validators
{
    public class ReactionTypeUpdateDtoValidator : AbstractValidator<ReactionTypeUpdateDto>
    {
        public ReactionTypeUpdateDtoValidator()
        {
            RuleFor(x => x.TypeName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(3);
        }
    }
}
