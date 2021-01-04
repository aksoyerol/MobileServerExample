using AndroidKotlinServer.API.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidKotlinServer.API.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required!");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stock is required!");
            RuleFor(x => x.Color).NotEmpty().WithMessage("Color is required!");
            
        }
    }
}
