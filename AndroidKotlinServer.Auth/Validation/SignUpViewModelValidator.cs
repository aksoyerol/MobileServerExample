using AndroidKotlinServer.Auth.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidKotlinServer.Auth.Validation
{
    public class SignUpViewModelValidator : AbstractValidator<SignUpViewModel>
    {
        public SignUpViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username area is required!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email area is required!").EmailAddress().WithMessage("Please enter a valid email adress!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
            RuleFor(x => x.City).NotEmpty().WithMessage("City area is required");
        }
    }
}
