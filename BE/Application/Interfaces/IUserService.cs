using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterCustomerViewModel registerViewModel);
        Task<bool> RegisterEmployee(RegisterEmployeeViewModel registerEmployeeViewModel);
        Task<List<UserViewModel>> GetAllAsync();
        Task<ResponseLoginViewModel> LoginAsync(LoginViewModel loginViewModel);
    }
}
