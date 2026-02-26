using EnterpriseAPI.Application.DTOs;
using EnterpriseAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAPI.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize] // Tüm aksiyonlar için yetki gerekli
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm ürünleri getirir
    /// </summary>
    /// <returns>Ürün listesi</returns>
    [HttpGet]
    [AllowAnonymous] // Herkes görebilir
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// ID'ye göre ürün getirir
    /// </summary>
    /// <param name="id">Ürün ID</param>
    /// <returns>Ürün detayı</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
            return NotFound();
            
        return Ok(product);
    }

    /// <summary>
    /// Yeni ürün oluşturur
    /// </summary>
    /// <param name="createDto">Ürün bilgileri</param>
    /// <returns>Oluşturulan ürün</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
    {
        var product = await _productService.CreateProductAsync(createDto);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Ürün günceller
    /// </summary>
    /// <param name="id">Güncellenecek ürün ID</param>
    /// <param name="updateDto">Güncel ürün bilgileri</param>
    /// <returns>İşlem sonucu</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateDto)
    {
        await _productService.UpdateProductAsync(id, updateDto);
        return NoContent();
    }

    /// <summary>
    /// Ürün siler (soft delete)
    /// </summary>
    /// <param name="id">Silinecek ürün ID</param>
    /// <returns>İşlem sonucu</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}