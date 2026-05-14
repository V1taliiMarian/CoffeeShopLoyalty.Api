using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoffeeShopLoyalty.Api.Data;
using CoffeeShopLoyalty.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShopLoyalty.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly string _botToken = "8701633374:AAG3xQ3UhReoNxnvlz6t101rXBmKtSasbrc"; // Твій токен

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
            Account account = new Account("dojoliqlm", "481415461142432", "qUNM3iWkUcoDNNO45853Gm3D6CE");
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        // ================= МЕНЮ =================

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] string name, [FromForm] int categoryId, [FromForm] decimal price, [FromForm] decimal costPrice, [FromForm] string? description, [FromForm] string? weightOrVolume, [FromForm] bool containsGluten, [FromForm] bool containsLactose, [FromForm] bool isSpicy, [FromForm] bool isNew, IFormFile? imageFile)
        {
            string? imageUrl = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams() { File = new FileDescription(imageFile.FileName, stream), Folder = "CoffeShopAPI", Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("auto") };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error == null) imageUrl = uploadResult.SecureUrl.ToString();
            }

            var newItem = new MenuItem { Name = name, CategoryId = categoryId, Price = price, CostPrice = costPrice, Description = description, WeightOrVolume = weightOrVolume, ContainsGluten = containsGluten, ContainsLactose = containsLactose, IsSpicy = isSpicy, IsNew = isNew, ImageUrl = imageUrl, IsAvailable = true };
            _context.MenuItems.Add(newItem);
            await _context.SaveChangesAsync();
            return Ok(newItem);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] string name, [FromForm] int categoryId, [FromForm] decimal price, [FromForm] decimal costPrice, [FromForm] string? description, [FromForm] string? weightOrVolume, [FromForm] bool containsGluten, [FromForm] bool containsLactose, [FromForm] bool isSpicy, [FromForm] bool isNew, IFormFile? imageFile)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();

            item.Name = name; item.CategoryId = categoryId; item.Price = price; item.CostPrice = costPrice; item.Description = description;
            item.WeightOrVolume = weightOrVolume; item.ContainsGluten = containsGluten; item.ContainsLactose = containsLactose; item.IsSpicy = isSpicy; item.IsNew = isNew;

            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams() { File = new FileDescription(imageFile.FileName, stream), Folder = "CoffeShopAPI", Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("auto") };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error == null) item.ImageUrl = uploadResult.SecureUrl.ToString();
            }

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // ================= НОВИНИ =================

        [HttpGet("News")]
        public async Task<IActionResult> GetNews() => Ok(await _context.News.OrderByDescending(n => n.CreatedAt).ToListAsync());

        [HttpPost("AddNews")]
        public async Task<IActionResult> AddNews([FromForm] string title, IFormFile? imageFile)
        {
            string? imageUrl = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams() { File = new FileDescription(imageFile.FileName, stream), Folder = "CoffeShopAPI/News", Transformation = new Transformation().Width(600).Height(300).Crop("fill").Gravity("auto") };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error == null) imageUrl = uploadResult.SecureUrl.ToString();
            }
            var news = new News { Title = title, ImageUrl = imageUrl };
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            // АВТО-РОЗСИЛКА НОВИНИ КОРИСТУВАЧАМ
            var customers = await _context.Customers.Where(c => c.TelegramId != 0 && c.TelegramId != 123456789).Select(c => c.TelegramId).Distinct().ToListAsync();
            string msg = $"🔥 <b>НОВИНА ВІД ROAST POINT:</b>\n\n{title}";

            _ = Task.Run(async () => {
                foreach (var tgId in customers)
                {
                    await SendTelegramMessage(tgId, msg, imageUrl);
                    await Task.Delay(50); // Уникаємо блокування від Telegram (ліміт 30 повідомлень в секунду)
                }
            });

            return Ok(news);
        }

        [HttpDelete("DeleteNews/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return NotFound();
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // ================= РОЗСИЛКА (BROADCAST) =================

        [HttpPost("Broadcast")]
        public async Task<IActionResult> BroadcastMessage([FromForm] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return BadRequest("Повідомлення порожнє");

            var customers = await _context.Customers.Where(c => c.TelegramId != 0 && c.TelegramId != 123456789).Select(c => c.TelegramId).Distinct().ToListAsync();
            int sentCount = 0;

            foreach (var tgId in customers)
            {
                bool success = await SendTelegramMessage(tgId, message);
                if (success) sentCount++;
                await Task.Delay(50);
            }

            return Ok(new { Sent = sentCount, Total = customers.Count });
        }

        private async Task<bool> SendTelegramMessage(long chatId, string text, string? imageUrl = null)
        {
            try
            {
                using var httpClient = new HttpClient();
                string url;
                string jsonPayload;

                var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    url = $"https://api.telegram.org/bot{_botToken}/sendPhoto";
                    jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, photo = imageUrl, caption = text, parse_mode = "HTML" }, options);
                }
                else
                {
                    url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                    jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, text = text, parse_mode = "HTML" }, options);
                }

                var response = await httpClient.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ================= АКЦІЇ =================

        [HttpGet("Promotions")]
        public async Task<IActionResult> GetPromotions() => Ok(await _context.Promotions.OrderByDescending(p => p.Id).ToListAsync());

        [HttpPost("AddPromotion")]
        public async Task<IActionResult> AddPromotion([FromBody] Promotion promo)
        {
            _context.Promotions.Add(promo);
            await _context.SaveChangesAsync();
            return Ok(promo);
        }

        [HttpDelete("DeletePromotion/{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var p = await _context.Promotions.FindAsync(id);
            if (p == null) return NotFound();
            _context.Promotions.Remove(p);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // ================= ДАНІ =================

        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders.Include(o => o.Customer).Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).OrderByDescending(o => o.CreatedAt).Take(100).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("Customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _context.Customers.OrderByDescending(c => c.Id).ToListAsync();
            return Ok(customers);
        }
    }
}