﻿
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        //Task<ResponseDto?> GetProductAsync(string couponCode);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductsAsync(ProductDto productDtos);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
