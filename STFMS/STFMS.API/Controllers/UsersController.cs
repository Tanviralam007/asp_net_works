using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using STFMS.BLL.Interfaces.Services;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.User;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
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
        // get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var usersDto = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

                return Ok(ApiResponseDTO<IEnumerable<UserResponseDTO>>.SuccessResponse(
                    usersDto,
                    "Users retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving users",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/{id}
        // get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {id} does not exist" }
                    ));
                }

                var userDto = _mapper.Map<UserResponseDTO>(user);

                return Ok(ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                    userDto,
                    "User retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/email/{email}
        // get user by email
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with email {email} does not exist" }
                    ));
                }

                var userDto = _mapper.Map<UserResponseDTO>(user);

                return Ok(ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                    userDto,
                    "User retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/type/{userType}
        // get users by type (Admin, Driver, Customer)
        [HttpGet("type/{userType}")]
        public async Task<IActionResult> GetUsersByType(UserType userType)
        {
            try
            {
                var users = await _userService.GetUsersByTypeAsync(userType);
                var usersDto = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

                return Ok(ApiResponseDTO<IEnumerable<UserResponseDTO>>.SuccessResponse(
                    usersDto,
                    $"{userType} users retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving users",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/active
        // get all active users
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var users = await _userService.GetActiveUsersAsync();
                var usersDto = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

                return Ok(ApiResponseDTO<IEnumerable<UserResponseDTO>>.SuccessResponse(
                    usersDto,
                    "Active users retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving active users",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/inactive
        // get all inactive users
        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            try
            {
                var users = await _userService.GetInactiveUsersAsync();
                var usersDto = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

                return Ok(ApiResponseDTO<IEnumerable<UserResponseDTO>>.SuccessResponse(
                    usersDto,
                    "Inactive users retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving inactive users",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/{id}/with-bookings
        // get user with their booking history
        [HttpGet("{id}/with-bookings")]
        public async Task<IActionResult> GetUserWithBookings(int id)
        {
            try
            {
                var user = await _userService.GetUserWithBookingsAsync(id);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {id} does not exist" }
                    ));
                }

                var userDto = _mapper.Map<UserWithDetailsResponseDTO>(user);

                return Ok(ApiResponseDTO<UserWithDetailsResponseDTO>.SuccessResponse(
                    userDto,
                    "User with bookings retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user with bookings",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/{id}/with-driver
        // get user with driver details (if user is a driver)
        [HttpGet("{id}/with-driver")]
        public async Task<IActionResult> GetUserWithDriver(int id)
        {
            try
            {
                var user = await _userService.GetUserWithDriverAsync(id);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {id} does not exist" }
                    ));
                }

                var userDto = _mapper.Map<UserWithDetailsResponseDTO>(user);

                return Ok(ApiResponseDTO<UserWithDetailsResponseDTO>.SuccessResponse(
                    userDto,
                    "User with driver details retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving user with driver details",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Users
        // create a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (request.UserType == UserType.Admin)
                {
                    return Unauthorized(ApiResponseDTO<string>.ErrorResponse(
                        "User creation failed",
                        new List<string> { "Admin accounts can only be created by existing admins" }
                    ));
                }

                // check if email already exists
                if (await _userService.EmailExistsAsync(request.Email))
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                        "User creation failed",
                        new List<string> { "Email is already registered" }
                    ));
                }

                // check if phone already exists
                if (await _userService.PhoneExistsAsync(request.PhoneNumber))
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                        "User creation failed",
                        new List<string> { "Phone number is already registered" }
                    ));
                }

                // map request to entity
                var user = _mapper.Map<User>(request);

                // create user
                var createdUser = await _userService.CreateUserAsync(user);

                // map to response DTO
                var userDto = _mapper.Map<UserResponseDTO>(createdUser);

                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = createdUser.UserId },
                    ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                        userDto,
                        "User created successfully"
                    )
                );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "User creation failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while creating user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/Users/{id}
        // update an existing user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                // get existing user
                var existingUser = await _userService.GetUserByIdAsync(id);

                if (existingUser == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {id} does not exist" }
                    ));
                }

                // map request to entity
                _mapper.Map(request, existingUser);
                existingUser.UserId = id; // ensure ID is not changed

                // update user
                await _userService.UpdateUserAsync(existingUser);

                // get updated user
                var updatedUser = await _userService.GetUserByIdAsync(id);
                var userDto = _mapper.Map<UserResponseDTO>(updatedUser);

                return Ok(ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                    userDto,
                    "User updated successfully"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "User update failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while updating user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/Users/{id}
        // delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {id} does not exist" }
                    ));
                }

                await _userService.DeleteUserAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "User deleted successfully",
                    "User deletion successful"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                    "User deletion failed",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deleting user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Users/{id}/activate
        // activate a user account
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                await _userService.ActivateUserAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "User activated successfully",
                    "User activation successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "User not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while activating user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PATCH: api/Users/{id}/deactivate
        // deactivate a user account
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                await _userService.DeactivateUserAsync(id);

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "User deactivated successfully",
                    "User deactivation successful"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponseDTO<string>.ErrorResponse(
                    "User not found",
                    new List<string> { ex.Message }
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while deactivating user",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/statistics/total
        // get total user count
        [HttpGet("statistics/total")]
        public async Task<IActionResult> GetTotalUserCount()
        {
            try
            {
                var count = await _userService.GetTotalUsersCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total user count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total user count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/statistics/customers
        // get total customer count
        [HttpGet("statistics/customers")]
        public async Task<IActionResult> GetTotalCustomerCount()
        {
            try
            {
                var count = await _userService.GetTotalCustomersCountAsync();

                return Ok(ApiResponseDTO<int>.SuccessResponse(
                    count,
                    "Total customer count retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving total customer count",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Users/search/{name}
        // search users by name
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchUsersByName(string name)
        {
            try
            {
                var users = await _userService.SearchUsersByNameAsync(name);
                var usersDto = _mapper.Map<IEnumerable<UserResponseDTO>>(users);

                return Ok(ApiResponseDTO<IEnumerable<UserResponseDTO>>.SuccessResponse(
                    usersDto,
                    $"Found {usersDto.Count()} user(s) matching '{name}'"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while searching users",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}