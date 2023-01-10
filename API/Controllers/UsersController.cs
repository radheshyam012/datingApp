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
    
    public class UsersController : BaseApiController
    {
        
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        
        public UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            this._photoService = photoService;
            this._mapper = mapper;
            this._userRepository = userRepository;
            
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var user =  await _userRepository.GetUsersAsync(); 

            var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);

            return Ok(userToReturn);

            
        }

        [HttpGet("{username}")]

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberDto memberDto)
        {
           // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();
            _mapper.Map(memberDto ,user);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed User Update");
        } 

        [HttpPost("add-photo")]
        
        public async Task<ActionResult<PhotoDtos>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null ) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsolutePath,
                PublicId = result.PublicId
            };
            if(user.Photos.Count ==0 ) photo.IsMain = true;
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDtos>(photo);

            return BadRequest("Problam add photo");

        }
    }
}