using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using System.Threading.Tasks;
using api.Services;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _itokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService itokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _itokenService = itokenService; 
            _signInManager = signInManager;
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == logindto.UserName.ToLower());

            if (user == null)
                return Unauthorized("Invalid Username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, logindto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid credentials ");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _itokenService.CreateToken(user)
                }
            );
        }
    }
}
