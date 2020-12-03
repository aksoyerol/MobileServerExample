using AndroidKotlinServer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidKotlinServer.Shared
{
    public static class UseCustomValidationResponseHandler
    {
        public static void UseCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            options.InvalidModelStateResponseFactory = context =>
              {
                  var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                  ErrorDto errorDto = new ErrorDto();
                  errorDto.Errors.AddRange(errors);
                  errorDto.StatusCode = 500;
                  errorDto.IsShow = false;

                  return new BadRequestObjectResult(errorDto);
              }
            );

        }
    }
}
