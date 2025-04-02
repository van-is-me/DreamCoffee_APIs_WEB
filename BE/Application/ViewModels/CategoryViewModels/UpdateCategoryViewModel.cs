using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.CategoryViewModels
{
    public class UpdateCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TypeCategory TypeCategory { get; set; }
    }
}
