using EnterpriseAPI.Domain.Common;

namespace EnterpriseAPI.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? Sku { get; set; }
    
    // Domain iş kuralları
    public bool IsInStock() => StockQuantity > 0;
    
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stok miktarı negatif olamaz");
            
        StockQuantity = quantity;
    }
}