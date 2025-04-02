using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.CategoryViewModels
{
    public class CreateCategoryViewModel
    {
        public string Name { get; set; }
        public TypeCategory TypeCategory { get; set; }
    }
}
