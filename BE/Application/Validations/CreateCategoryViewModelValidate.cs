using Application.Interfaces;
using Application.ViewModels.CategoryViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{

    public class CreateCategoryViewModelValidate : AbstractValidator<CreateCategoryViewModel>
    {
        //private readonly ICurrentTime _currentTime;
        //private readonly IClaimsService _claimsService;
        public CreateCategoryViewModelValidate()
        {
            //_currentTime = currentTime;
            //_claimsService = claimsService;

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không thể bỏ trống");
            RuleFor(x => x.TypeCategory).NotEmpty().WithMessage("Thể loại không thể bỏ trống");
            //_claimsService = claimsService;
        }
    }
}
