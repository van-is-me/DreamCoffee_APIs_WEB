using Application.Interfaces;
using Application.Services;
using Application.ViewModels.CategoryViewModels;
using Application.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // POST: api/Product
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProductViewModel)
        {
            try
            {
                var result = await _productService.CreateProduct(createProductViewModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        // GET: api/Product
        [AllowAnonymous]
        [HttpGet("getByCategory")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetProductByCategory(categoryId);
            return Ok(products);
        }
    }
}