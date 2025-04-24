using Application.Interfaces;
using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.ProductViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTimeService _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductService(IUnitOfWork unitOfWork, ICurrentTimeService currentTime, IClaimsService claimsService, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task<bool> CreateProduct(CreateProductViewModel createProductViewModel)
        {
            var mapper = _mapper.Map<Product>(createProductViewModel);
            await _unitOfWork.ProductRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo sản phẩm thất bại");
        }
    

        public async Task<List<ProductViewModel>> GetProductByCategory(Guid categoryId)
        {
            var products = await _unitOfWork.ProductRepository.GetByCategory(categoryId);

            var result = _mapper.Map<List<ProductViewModel>>(products);

            return result;
        }
    }

}
