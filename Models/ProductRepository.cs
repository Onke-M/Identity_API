using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProductRepository(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
        public async Task<Brand[]> GetAllBrandsAsync()
        {
            IQueryable<Brand> query = _appDbContext.Brands;
            return await query.ToArrayAsync();
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            IQueryable<Product> query = _appDbContext.Products;
            return await query.ToArrayAsync();
        }

        public async Task<ProductType[]> GetAllProductTypesAsync()
        {
            IQueryable<ProductType> query = _appDbContext.ProductTypes;
            return await query.ToArrayAsync();
        }

        //public IEnumerable<Product> GetFullProducts()
        //{
        //    var prodList = (from p in _appDbContext.Products
        //                    join pb in _appDbContext.Brands on p.Brand equals pb.BrandId
        //                    join pt in _appDbContext.ProductTypes on p.ProductType equals pt.ProductTypeId
        //                    select new Product()
        //                    {
        //                        ProductId = p.ProductId,
        //                        Name = p.Name,
        //                        Price = p.Price,
        //                        Description = p.Description,
        //                        Brand = pb.Name,
        //                        ProductType = pt.Name,
        //                        IsActive = p.IsActive,
        //                        IsDeleted = p.IsDeleted,
        //                        DateCreated = p.DateCreated,
        //                        DateModified = p.DateModified
        //                    }).ToList();
        //    return prodList;
        //}
    }
}
