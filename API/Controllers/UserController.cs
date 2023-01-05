// using API.Data;
// using API.Entities;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// //{token = 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}
// namespace API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")] //get/userController
//     public class UsersController : ControllerBase
//     {
//         private readonly DataContext _context;
//         public UsersController(DataContext context)
//         {
//             _context = context;
            
//         }

//         public UsersController()
//         {
//         }

//         [HttpGet]
//          public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
//          {
//             var users = await _context.Users.ToListAsync();
//             return users;
//          }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<AppUser>> GetUser(int id)
//         {
//             return await _context.Users.FindAsync(id);
           
//         }
        
//     }
// }