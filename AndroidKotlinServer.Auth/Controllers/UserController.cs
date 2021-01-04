using AndroidKotlinServer.Auth.Dtos;
using AndroidKotlinServer.Auth.Models;
using AndroidKotlinServer.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace AndroidKotlinServer.Auth.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public IActionResult Test()
        {
            return Ok("test ok");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {

            var user = new ApplicationUser
            {
                City = signUpViewModel.City,
                UserName = signUpViewModel.UserName,
                Email = signUpViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, signUpViewModel.Password);
            if (!result.Succeeded)
            {
                ErrorDto errorDto = new ErrorDto();
                errorDto.StatusCode = 400;
                errorDto.IsShow = true;
                errorDto.Errors.AddRange(result.Errors.Select(x => x.Description).ToList());

                //TO:DO hata mesajı
                return BadRequest(errorDto);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);//Sub burada jwt'nin içerisindeki userId oluyor.
            if (userIdClaim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null) return BadRequest();

            var userDto = new ApplicationUserDto
            {
                UserName = user.UserName,
                City = user.City,
                Email = user.Email
            };

            return Ok(userDto);   

        }
    }
}
