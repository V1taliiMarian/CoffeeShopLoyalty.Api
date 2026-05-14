using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoffeeShopLoyalty.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public string OrderType { get; set; } = "В закладі";

        // НОВЕ: Номер столика, з якого зроблено замовлення
        public string? TableNumber { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal BonusUsed { get; set; }
        public decimal FinalAmount { get; set; }

        // НОВЕ: Статус самого замовлення (Open - відкритий рахунок, Completed - завершено, Cancelled - скасовано)
        public string OrderStatus { get; set; } = "Open";
        //ОЦІНКА ЗАМОВЛЕННЯ (0-5)
        public decimal TipAmount { get; set; } = 0;
        public int Rating { get; set; } = 0;

        // ЗМІНЕНО: За замовчуванням тепер неоплачено
        public string PaymentStatus { get; set; } = "Unpaid";

        // ЗМІНЕНО: Готівка, Картка або None (поки не оплачено)
        public string PaymentMethod { get; set; } = "None";

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [JsonIgnore]
        public Customer? Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
        public List<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new();
    }
}