using FluentValidation;
using ReactionService.Models.DTOs.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Validators
{
    public class ReactionCreationDtoValidator : AbstractValidator<ReactionCreationDto>
    {
        public ReactionCreationDtoValidator()
        {
            RuleFor(x => x.PostId).NotNull().NotEmpty();
        }
    }
}
