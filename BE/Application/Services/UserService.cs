using Application.Interfaces;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using BCrypt.Net;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public UserService(IUnitOfWork unitOfWork, ICurrentTimeService currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;

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
    }
}
