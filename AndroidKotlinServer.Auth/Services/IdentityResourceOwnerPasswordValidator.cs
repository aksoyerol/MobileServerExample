using AndroidKotlinServer.Auth.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidKotlinServer.Auth.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);
            if (existUser == null)
            {
                var error = new Dictionary<string, object>();
                error.Add("errors", new List<string> { "Wrong email or pass!" });
                error.Add("status", 400);
                error.Add("isShow", true);
                context.Result.CustomResponse = error;
                return;
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser,context.Password);
            if (passwordCheck == false) {
                var error = new Dictionary<string, object>();
                error.Add("errors", new List<string> { "Wrong email or pass!" });
                error.Add("status", 400);
                error.Add("isShow", true);
                context.Result.CustomResponse = error;
                return;
            }

            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            
        }
    }
}
