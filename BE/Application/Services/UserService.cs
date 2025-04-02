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
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;

        }
        public async Task<bool> CreateUser(CreateUserViewModel createUserViewModel)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserViewModel.PasswordHash);
                var user = _mapper.Map<User>(createUserViewModel);
                user.PasswordHash = hashedPassword;
                await _unitOfWork.UserRepository.AddAsync(user);
                return await _unitOfWork.SaveChangeAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo người dùng: {ex.Message}", ex);
            }
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            var userViewModels = _mapper.Map<List<UserViewModel>>(users);

            return userViewModels;
        }
    }
}
