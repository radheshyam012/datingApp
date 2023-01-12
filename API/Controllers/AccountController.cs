using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        
        private readonly ITokenServices _tokenServices;
        public AccountController(UserManager<AppUser> userManager,ITokenServices tokenServices)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)

        {
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user =new AppUser
            {
                UserName = registerDto.Username.ToLower(),
            };
            var result =await _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user,"Member");
            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            
            return new UserDto
            {
                Username = user.UserName,
                Token =await _tokenServices.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => 
                x.UserName == loginDto.Username);
            if(user == null) return Unauthorized("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user,loginDto.Password);

            if(!result) return Unauthorized("Invalid Password");
            
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenServices.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }
    }
}