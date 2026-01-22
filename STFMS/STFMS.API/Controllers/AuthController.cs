using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STFMS.API.DTOs.Common;
using STFMS.API.DTOs.User;
using STFMS.BLL.Interfaces.Services;
using STFMS.DAL.Entities;

namespace STFMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // POST: api/Auth/register
        // register a new customer account
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request.UserType == UserType.Admin)
                {
                    return Unauthorized(ApiResponseDTO<string>.ErrorResponse(
                        "Registration Failed",
                        new List<string> { "Admin accounts cannot be self-registered. Contact system administrator." }
                    ));
                }

                // if email already exists
                if (await _userService.EmailExistsAsync(request.Email))
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                        "Registration Failed",
                        new List<string> { "Email is already registered" }
                    ));
                }

                // check if phone number already exists
                if (await _userService.PhoneExistsAsync(request.PhoneNumber))
                {
                    return BadRequest(ApiResponseDTO<string>.ErrorResponse(
                        "Registration Failed",
                        new List<string> { "Phone number already registered" }
                    ));
                }

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = request.Password,
                    PhoneNumber = request.PhoneNumber,
                    UserType = request.UserType,  
                    RegisteredDate = DateTime.UtcNow,
                    IsActive = true,
                };

                // create user
                var createdUser = await _userService.CreateUserAsync(user);

                // map
                var userResponse = _mapper.Map<UserResponseDTO>(createdUser);

                return Ok(ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                    userResponse,
                    $"{request.UserType} account registered successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred during registration",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Auth/login
        // authenticate user and return user details
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // validate user credentials
                var user = await _userService.ValidateUserCredentialsAsync(request.Email, request.Password);

                if (user == null)
                {
                    return Unauthorized(ApiResponseDTO<string>.ErrorResponse(
                        "Login failed",
                        new List<string> { "Invalid email or password" }
                    ));
                }

                // check if user is active
                if (!user.IsActive)
                {
                    return Unauthorized(ApiResponseDTO<string>.ErrorResponse(
                        "Login failed",
                        new List<string> { "Account is inactive. Please contact support." }
                    ));
                }

                // create login response
                var loginResponse = new LoginResponseDTO
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    UserType = user.UserType.ToString(),
                    // token = "temporary-token", // pore implement korum
                    LoginTime = DateTime.UtcNow
                };

                return Ok(ApiResponseDTO<LoginResponseDTO>.SuccessResponse(
                    loginResponse,
                    "Login successful"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred during login",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Auth/check-email/{email}
        // check if email is already registered
        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            try
            {
                var exists = await _userService.EmailExistsAsync(email);

                return Ok(ApiResponseDTO<bool>.SuccessResponse(
                    exists,
                    exists ? "Email is already registered" : "Email is available"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while checking email",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Auth/check-phone/{phone}
        // check if phone number is already registered
        [HttpGet("check-phone/{phone}")]
        public async Task<IActionResult> CheckPhoneExists(string phone)
        {
            try
            {
                var exists = await _userService.PhoneExistsAsync(phone);

                return Ok(ApiResponseDTO<bool>.SuccessResponse(
                    exists,
                    exists ? "Phone number is already registered" : "Phone number is available"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while checking phone number",
                    new List<string> { ex.Message }
                ));
            }
        }

        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> Logout(int userId)
        {
            try
            {
                // Verify user exists
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "Logout failed",
                        new List<string> { "User not found" }
                    ));
                }

                // TODO: jwt implement korte hobe
                // for now, just log the action (optional)

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    $"User {user.FullName} logged out successfully",
                    "Logout successful"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred during logout",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/Auth/change-password
        // change user password (requires user ID in request)
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequestDTO request)
        {
            try
            {
                // TODO: Implement password change logic
                // This is a placeholder implementation

                return Ok(ApiResponseDTO<string>.SuccessResponse(
                    "Password changed successfully",
                    "Password update successful"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while changing password",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/Auth/profile/{userId}
        // get user profile by ID - temporary jwt pore implement korum
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(ApiResponseDTO<string>.ErrorResponse(
                        "User not found",
                        new List<string> { $"User with ID {userId} does not exist" }
                    ));
                }

                var userResponse = _mapper.Map<UserResponseDTO>(user);

                return Ok(ApiResponseDTO<UserResponseDTO>.SuccessResponse(
                    userResponse,
                    "Profile retrieved successfully"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseDTO<string>.ErrorResponse(
                    "An error occurred while retrieving profile",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}