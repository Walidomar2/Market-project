using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _itokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService itokenService)
        {
            _userManager = userManager;
            _itokenService = itokenService; 
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDtoModel)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDtoModel.UserName,
                    Email = registerDtoModel.Email, 
                };

                var CreatedUser = await _userManager.CreateAsync(appUser);

                if(CreatedUser.Succeeded)
                {
                    var UserRole = await _userManager.AddToRoleAsync(appUser,"User");
                    if (UserRole.Succeeded)
                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _itokenService.CreateToken(appUser)
                        }) ;
                    else
                        return StatusCode(500, CreatedUser.Errors);
                }
                else
                {
                    return StatusCode(500, CreatedUser.Errors);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
