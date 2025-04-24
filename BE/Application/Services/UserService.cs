using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using BCrypt.Net;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTimeService _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; 

        public UserService(IUnitOfWork unitOfWork, ICurrentTimeService currentTime, IClaimsService claimsService, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            return userViewModels;
        }

        public async Task<bool> RegisterUser(RegisterCustomerViewModel registerViewModel)
        {
            try
            {
                // Băm mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerViewModel.PasswordHash);

                // Map RegisterViewModel thành User
                var user = _mapper.Map<User>(registerViewModel);

                //gán auto role customer
                user.Role = Domain.Enums.UserRole.Customer;

                // Gán mật khẩu đã băm cho người dùng
                user.PasswordHash = hashedPassword;

                // Thêm người dùng vào cơ sở dữ liệu
                await _unitOfWork.UserRepository.AddAsync(user);

                // Lưu thay đổi vào cơ sở dữ liệu
                return await _unitOfWork.SaveChangeAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo người dùng: {ex.Message}", ex);
            }
        }



       

        public async Task<bool> RegisterEmployee(RegisterEmployeeViewModel registerEmployeeViewModel)
        {
            try
            {
                // Băm mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerEmployeeViewModel.PasswordHash);

                // Map RegisterViewModel thành User
                var user = _mapper.Map<User>(registerEmployeeViewModel);

                // Gán mật khẩu đã băm cho người dùng
                user.PasswordHash = hashedPassword;

                // Thêm người dùng vào cơ sở dữ liệu
                await _unitOfWork.UserRepository.AddAsync(user);

                // Lưu thay đổi vào cơ sở dữ liệu
                return await _unitOfWork.SaveChangeAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo người dùng: {ex.Message}", ex);
            }
        }

        public async Task<ResponseLoginViewModel> LoginAsync(LoginViewModel loginViewModel)
        {
            // so sánh username
            var user = _unitOfWork.UserRepository
                .GetAllAsync().Result.FirstOrDefault(u => u.Username == loginViewModel.Username);

            if (user == null)
            {
                throw new HttpResponseException(404, "Tài khoản không tồn tại.");
            }

            // So sánh mật khẩu đã băm bằng BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginViewModel.PasswordHash, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new HttpResponseException(403, "Sai mật khẩu.");
                //Người dùng có tồn tại, nhưng không được phép truy cập với thông tin hiện tại (mật khẩu sai).
            }

            // Tạo JWT Token nếu đúng
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["Jwt:SecretKey"]; 
            var keyBytes = Encoding.UTF8.GetBytes(secretKey); // chuyển string secret key thành byte[], vì JWT library yêu cầu định dạng đó

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(3), // thời gian hết hạn của token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var response = _mapper.Map<ResponseLoginViewModel>(user);
            response.Token = jwtToken;

            return response;
        }
    }
}
