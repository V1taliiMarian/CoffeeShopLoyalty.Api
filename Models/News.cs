using System;

namespace CoffeeShopLoyalty.Api.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // Фото з Cloudinary
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}