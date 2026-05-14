using CoffeeShopLoyalty.Api.Data;
using CoffeeShopLoyalty.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopLoyalty.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Отримати всі товари. Обов'язково додано customerId
        [HttpGet]
        public async Task<IActionResult> GetMenuItems([FromQuery] int customerId)
        {
            // ОНОВЛЕНО: Жорстке сортування товарів
            var items = await _context.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsAvailable)
                .OrderBy(m => m.CategoryId) // Спочатку сортуємо за ID категорії (1, 2, 3...)
                .ThenBy(m => m.Id)          // Потім за ID самого товару в межах категорії
                .ToListAsync();

            // Отримуємо ID товарів, які лайкнув цей клієнт
            var likedItemIds = await _context.CustomerMenuItemLikes
                .Where(l => l.CustomerId == customerId)
                .Select(l => l.MenuItemId)
                .ToListAsync();

            // Створюємо анонімний об'єкт, щоб додати властивість IsLikedByUser
            var result = items.Select(item => new
            {
                item.Id,
                item.CategoryId,
                item.Category,
                item.Name,
                item.Description,
                item.ImageUrl,
                item.Price,
                item.CostPrice,
                item.WeightOrVolume,
                item.ContainsGluten,
                item.ContainsLactose,
                item.IsSpicy,
                item.IsNew,
                item.TotalLikes,
                IsLikedByUser = likedItemIds.Contains(item.Id)
            });

            return Ok(result);
        }

        // Поставити лайк
        [HttpPost("{itemId}/like")]
        public async Task<IActionResult> LikeItem(int itemId, [FromQuery] int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound("Клієнта не знайдено");

            var item = await _context.MenuItems.FindAsync(itemId);
            if (item == null) return NotFound("Товар не знайдено");

            // Перевіряємо, чи вже стоїть лайк (щоб уникнути помилки унікальності)
            var existingLike = await _context.CustomerMenuItemLikes
                .FirstOrDefaultAsync(l => l.CustomerId == customerId && l.MenuItemId == itemId);

            if (existingLike == null)
            {
                // Додаємо запис про лайк
                _context.CustomerMenuItemLikes.Add(new CustomerMenuItemLike { CustomerId = customerId, MenuItemId = itemId });
                // Інкрементуємо загальний лічильник у товарі
                item.TotalLikes++;
                await _context.SaveChangesAsync();
                return Ok(new { item.TotalLikes }); // Повертаємо оновлену кількість
            }

            return BadRequest("Лайк вже стоїть");
        }

        // Прибрати лайк
        [HttpDelete("{itemId}/unlike")]
        public async Task<IActionResult> UnlikeItem(int itemId, [FromQuery] int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) return NotFound("Клієнта не знайдено");

            var item = await _context.MenuItems.FindAsync(itemId);
            if (item == null) return NotFound("Товар не знайдено");

            // Шукаємо лайк
            var existingLike = await _context.CustomerMenuItemLikes
                .FirstOrDefaultAsync(l => l.CustomerId == customerId && l.MenuItemId == itemId);

            if (existingLike != null)
            {
                // Видаляємо запис про лайк
                _context.CustomerMenuItemLikes.Remove(existingLike);
                // Декрементуємо загальний лічильник у товарі
                item.TotalLikes = item.TotalLikes > 0 ? item.TotalLikes - 1 : 0;
                await _context.SaveChangesAsync();
                return Ok(new { item.TotalLikes }); // Повертаємо оновлену кількість
            }

            return BadRequest("Лайк не стоїть");
        }
    }
}