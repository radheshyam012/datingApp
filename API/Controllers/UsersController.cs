using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        
        
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;
        
        public UsersController(IUnitOfWork uow,IMapper mapper,IPhotoService photoService)
        {
            this._uow = uow;
            this._photoService = photoService;
            this._mapper = mapper;
           
            
            
        }
       // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var user =  await _uow.userRepository.GetUsersAsync(); 

            var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);

            return Ok(userToReturn);

            
        }
        
        [HttpGet("{username}")]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _uow.userRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberDto memberDto)
        {
           // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _uow.userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();
            _mapper.Map(memberDto ,user);

            if(await _uow.Complated()) return NoContent();
            return BadRequest("Failed User Update");
        } 

       


        // [HttpDelete("{id}")]
        // public async Task<ActionResult<AppUser>> DeleteUser(int id)
        // {
        //     var user =  await _userRepository.DeleteAyncs(id);
        //     if(user == null) return NotFound();
        //     return user;


        // }
            
          

        // [HttpPost("add-photo")]
        
        // public async Task<ActionResult<PhotoDtos>> AddPhoto(IFormFile file)
        // {
        //     var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        //     if(user == null ) return NotFound();

        //     var result = await _photoService.AddPhotoAsync(file);

        //     if(result.Error != null) return BadRequest(result.Error.Message);
        //     var photo = new Photo
        //     {
        //         Url = result.SecureUrl.AbsolutePath,
        //         PublicId = result.PublicId
        //     };
        //     if(user.Photos.Count ==0 ) photo.IsMain = true;
        //     user.Photos.Add(photo);
        //     if(await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDtos>(photo);

        //     return BadRequest("Problam add photo");

        // }
    }
}