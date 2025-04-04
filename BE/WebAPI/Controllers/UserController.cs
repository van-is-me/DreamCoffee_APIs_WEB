using Application.Interfaces;
using Application.Services;
using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<ActionResult<List<UserViewModel>>> GetAllAsync()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("registerCustomer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel registerViewModel)
        {
            try
            {
                var result = await _userService.RegisterUser(registerViewModel);

                if (result)
                {
                    // Nếu đăng ký thành công
                    return Ok(new { Message = "Customer registered successfully!" });
                }
                else
                {
                    // Nếu đăng ký không thành công (ví dụ: tên người dùng đã tồn tại)
                    return BadRequest(new { Message = "Username already exists. Please choose a different username." });
                }
            }
            catch (ArgumentException ex)
            {
                // Nếu lỗi đầu vào không hợp lệ
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Nếu có vấn đề liên quan đến hoạt động không hợp lệ
                return BadRequest(new { Message = "Invalid operation. Please try again later.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi phía server
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later.", Details = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("registerEmployee")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel registerEmployeeViewModel)
        {
            try
            {
                var result = await _userService.RegisterEmployee(registerEmployeeViewModel);

                if (result)
                {
                    // Nếu đăng ký thành công
                    return Ok(new { Message = "Employee registered successfully!" });
                }
                else
                {
                    // Nếu đăng ký không thành công (ví dụ: tên người dùng đã tồn tại)
                    return BadRequest(new { Message = "Username already exists. Please choose a different username." });
                }
            }
            catch (ArgumentException ex)
            {
                // Nếu lỗi đầu vào không hợp lệ
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Nếu có vấn đề liên quan đến hoạt động không hợp lệ
                return BadRequest(new { Message = "Invalid operation. Please try again later.", Details = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi phía server
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later.", Details = ex.Message });
            }
        }
    }
}
