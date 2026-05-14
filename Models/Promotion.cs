using System;

namespace CoffeeShopLoyalty.Api.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // "1+1=3" або "Discount"
        public string PromoType { get; set; } = "1+1=3";

        // Якщо null - діє на всі категорії
        public int? CategoryId { get; set; }

        // Використовується, якщо PromoType == "Discount" (наприклад 10 для 10%)
        public decimal DiscountPercent { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}