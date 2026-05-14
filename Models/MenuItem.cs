using System.Text.Json.Serialization;

namespace CoffeeShopLoyalty.Api.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsAvailable { get; set; }

        public string? WeightOrVolume { get; set; }
        public bool ContainsGluten { get; set; }
        public bool ContainsLactose { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsNew { get; set; }

        // НОВЕ: Загальна кількість лайків
        public int TotalLikes { get; set; } = 0;

        public Category? Category { get; set; }

        // НОВЕ: Зв'язок для визначення, хто лайкнув
        [JsonIgnore]
        public ICollection<CustomerMenuItemLike>? CustomerLikes { get; set; }
    }
}