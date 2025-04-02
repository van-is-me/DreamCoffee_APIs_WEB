using Application;
using Application.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _dbContext;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;

        private readonly IProductRepository _productRepository;

        private readonly IReviewRepository _reviewRepository;

        private readonly IShippingRepository _shippingRepository;

        private readonly ITransactionRepository _transactionRepository;

        private readonly IUserRepository _userRepository;

        public UnitOfWork(AppDBContext dbContext, ICategoryRepository categoryRepository, IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IReviewRepository reviewRepository,
            IShippingRepository shippingRepository, ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _shippingRepository = shippingRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public ICategoryRepository CategoryRepository => _categoryRepository;
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IProductRepository ProductRepository => _productRepository;
        public IReviewRepository ReviewRepository => _reviewRepository;
        public IShippingRepository ShippingRepository => _shippingRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IUserRepository UserRepository => _userRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose() => _dbContext.Dispose();
    }
}
