using FluentValidation;
using PostService.Models.DTOs.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Validators
{
    public class ServiceUpdateDtoValidator : AbstractValidator<ServiceUpdateDto>
    {
        public ServiceUpdateDtoValidator()
        {
            RuleFor(x => x.Category).NotNull().NotEmpty().MinimumLength(3).MaximumLength(40);
            RuleFor(x => x.PostName).NotNull().NotEmpty().MinimumLength(5).MaximumLength(50);
            RuleFor(x => x.AccountId).NotNull().NotEmpty();
        }
    }
}
