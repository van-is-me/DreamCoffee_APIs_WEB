using Application.Interfaces;
using Application.Validations;
using Application.ViewModels.CategoryViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTimeService _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, ICurrentTimeService currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateCategory(CreateCategoryViewModel createCategoryViewModel)
        {
            try
            {
                var validator = new CreateCategoryViewModelValidate();
                var validationResult = validator.Validate(createCategoryViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        throw new Exception(error.ErrorMessage);
                    }
                }

                var mapper = _mapper.Map<Category>(createCategoryViewModel);
                await _unitOfWork.CategoryRepository.AddAsync(mapper);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo danh mục thất bại");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo danh mục: {ex.Message}", ex);
            }
        }
    }
}
