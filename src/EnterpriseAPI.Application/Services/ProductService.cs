using EnterpriseAPI.Application.DTOs;
using EnterpriseAPI.Application.Interfaces;
using EnterpriseAPI.Domain.Entities;
using EnterpriseAPI.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EnterpriseAPI.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repository;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IGenericRepository<Product> repository,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator,
        ILogger<ProductService> logger)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        _logger.LogInformation("Tüm ürünler getiriliyor");
        var products = await _repository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        _logger.LogInformation("ID: {ProductId} olan ürün getiriliyor", id);
        var product = await _repository.GetByIdAsync(id);
        return product is not null ? MapToDto(product) : null;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createDto)
    {
        // Validasyon
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            Sku = createDto.Sku,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(product);
        _logger.LogInformation("Yeni ürün oluşturuldu: {ProductId}", created.Id);
        
        return MapToDto(created);
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            throw new KeyNotFoundException($"ID: {id} olan ürün bulunamadı");

        product.Name = updateDto.Name;
        product.Description = updateDto.Description;
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);
        _logger.LogInformation("Ürün güncellendi: {ProductId}", id);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null)
            throw new KeyNotFoundException($"ID: {id} olan ürün bulunamadı");

        // Soft delete (mantıksal silme)
        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(product);
        _logger.LogInformation("Ürün silindi (soft delete): {ProductId}", id);
    }

    public async Task<bool> ProductExistsAsync(Guid id)
    {
        return await _repository.ExistsAsync(p => p.Id == id);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity
        };
    }
}