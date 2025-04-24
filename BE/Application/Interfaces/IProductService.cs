using Application.ViewModels.ProductViewModels;
using Application.ViewModels.UserViewModel;
using Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{

    public interface IProductService
    {
        Task<bool> CreateProduct(CreateProductViewModel createProductViewModel);
        Task<List<ProductViewModel>> GetProductByCategory(Guid categoryId);
    }
}
