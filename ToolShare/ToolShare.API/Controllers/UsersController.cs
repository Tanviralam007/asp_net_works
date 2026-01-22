using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.User;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var usersDTO = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if(user == null) return NotFound();
                var userDTO = _mapper.Map<UserResponseDTO>(user);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/email/jalal@gmail.com
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if(user == null) return NotFound();
                var userDTO = _mapper.Map<UserResponseDTO>(user);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/role/1
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUserByRole(byte role)
        {
            try
            {
                if(role > 2) return BadRequest(new
                {
                    message = "Invalid role. Role must be 1 (Borrower), 2 (ToolOwner), or 3 (Admin)"
                });

                var users = await _userService.GetUsersByRoleAsync(role);
                var userDTO = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = _mapper.Map<User>(request);
                var createUser = await _userService.CreateUserAsync(user);
                var userDTO = _mapper.Map<UserResponseDTO>(createUser);
                return CreatedAtAction(nameof(GetUserById), new { id = userDTO.Id }, userDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (id != request.Id) return BadRequest(new { message = "ID in URL does not match ID in request body" });
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = _mapper.Map<User>(request);
                var updateUser = await _userService.UpdateUserAsync(user);
                var userDTO = _mapper.Map<UserResponseDTO>(updateUser);

                return Ok(userDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user", error = ex.Message });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound();

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Users/5/block
        [HttpPost("{userId}/block")]
        public async Task<ActionResult> BlockUser(int userId, [FromQuery] int adminId)
        {
            try
            {
                var result = await _userService.BlockUserAsync(userId, adminId);
                if (result) return Ok();
                return BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Users/5/unblock
        [HttpPost("{userId}/unblock")]
        public async Task<ActionResult> UnblockUser(int userId, [FromQuery] int adminId)
        {
            try
            {
                var result = await _userService.UnblockUserAsync(userId, adminId);
                if (result) return Ok();
                return BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalUsersCount()
        {
            try
            {
                var count = await _userService.GetTotalUsersCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Users/check-email?email=jalal@gmail.com
        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmailUnique([FromQuery] string email, [FromQuery] int? excludeUserId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email)) return BadRequest();
                var isUnique = await _userService.IsEmailUniqueAsync(email, excludeUserId);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
