using Application.Interfaces;
using Application.IRepositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ShippingRepository : GenericRepository<Shipping>, IShippingRepository
    {
        private readonly AppDBContext _dbContext;
        public ShippingRepository(AppDBContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
    }
}
