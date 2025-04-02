using Application.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<bool> CreateCategory(CreateCategoryViewModel createCategoryViewModel);
    }
}
