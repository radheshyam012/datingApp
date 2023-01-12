using API.Entities;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController :BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            
        }
        [Authorize(Policy ="RequiredAdminRole")]
        [HttpGet("user-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
            var user = await _userManager.Users.OrderBy(u=>u.UserName)
            .Select(u=>new{
                u.Id ,
                u.UserName,
                Roles = u.UserRoles.Select(r=> r.Roles.Name).ToList()
            }).ToListAsync();

            return Ok(user);
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRole(string username,[FromQuery]string role)
        {
            if(string.IsNullOrEmpty(role)) return BadRequest("You select atleast one role");
            var selectRoled = role.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);

            if(user == null )return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user,selectRoled.Except(userRoles));

            if(!result.Succeeded) return BadRequest("Failed to add to role");

            result = await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectRoled));
            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user)); 
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photo-to-moderate")]
        public ActionResult GetPhotoForModerator()
        {
            return Ok("Admin or moderator can see this");
        }
    }
}