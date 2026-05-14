using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoffeeShopLoyalty.Api.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public long TelegramId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; } = "Не вказано";
        public decimal TotalSpent { get; set; }
        public decimal BonusBalance { get; set; }
        public string LoyaltyLevel { get; set; } = "Standard";
        public DateTimeOffset? DateOfBirth { get; set; }

        // ПОЛЯ ДЛЯ РЕФЕРАЛКИ
        public string ReferralCode { get; set; } = "";
        public bool HasUsedReferral { get; set; } = false;

        // НОВЕ ПОЛЕ: Зберігає ID того, хто запросив, до моменту першої оплати
        public int? ReferredByCustomerId { get; set; }

        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();
        [JsonIgnore]
        public List<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new();
    }
}