using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3_API.Models
{
    public interface IProductRepository
    {
        Task<Product[]> GetAllProductsAsync();
        Task<Brand[]> GetAllBrandsAsync();
        Task<ProductType[]> GetAllProductTypesAsync();
        //IEnumerable<Product> GetFullProducts();

    }
}
