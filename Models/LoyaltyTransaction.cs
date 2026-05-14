using System;

namespace CoffeeShopLoyalty.Api.Models
{
    public class LoyaltyTransaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? OrderId { get; set; }
        public decimal PointsAdded { get; set; } = 0.00m;
        public decimal PointsDeducted { get; set; } = 0.00m;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string Reason { get; set; } = string.Empty;

        public Customer Customer { get; set; } = null!;
        public Order? Order { get; set; }
    }
}