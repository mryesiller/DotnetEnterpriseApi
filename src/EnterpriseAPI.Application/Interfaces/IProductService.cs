using EnterpriseAPI.Application.DTOs;

namespace EnterpriseAPI.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
    Task UpdateProductAsync(Guid id, UpdateProductDto updateDto);
    Task DeleteProductAsync(Guid id);
    Task<bool> ProductExistsAsync(Guid id);
}