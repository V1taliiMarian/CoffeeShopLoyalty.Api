using CoffeeShopLoyalty.Api.Data;
using CoffeeShopLoyalty.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoffeeShopLoyalty.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders() { return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).ToListAsync(); }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound(); return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id) return BadRequest();
            _context.Entry(order).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { if (!OrderExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            _context.Orders.Remove(order); await _context.SaveChangesAsync(); return NoContent();
        }

        private bool OrderExists(int id) => _context.Orders.Any(e => e.Id == id);

        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(int customerId)
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Where(o => o.CustomerId == customerId).OrderByDescending(o => o.Id).ToListAsync();
        }

        [HttpGet("Customer/{customerId}/Open")]
        public async Task<ActionResult<Order>> GetOpenOrder(int customerId)
        {
            var openOrder = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .Where(o => o.CustomerId == customerId && (o.OrderStatus == "Open" || o.OrderStatus == "WaitingForPayment" || o.OrderStatus == "Processing" || o.OrderStatus == "Ready") && o.PaymentStatus == "Unpaid")
                .FirstOrDefaultAsync();

            if (openOrder == null) return NotFound();
            return openOrder;
        }

        [HttpGet("Table/{tableNumber}/Check")]
        public async Task<ActionResult<bool>> CheckTable(string tableNumber, [FromQuery] int currentCustomerId)
        {
            var activeOrder = await _context.Orders
                .Where(o => o.TableNumber == tableNumber && (o.OrderStatus == "Open" || o.OrderStatus == "WaitingForPayment" || o.OrderStatus == "Processing" || o.OrderStatus == "Ready") && o.CustomerId != currentCustomerId)
                .FirstOrDefaultAsync();
            return Ok(activeOrder != null);
        }

        [HttpPost("{id}/Rate")]
        public async Task<IActionResult> RateOrder(int id, [FromBody] int rating)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Rating = rating; await _context.SaveChangesAsync(); return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null) return BadRequest("Клієнта не знайдено.");

            var existingOpenOrder = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerId == customer.Id && o.PaymentStatus == "Unpaid");
            if (existingOpenOrder != null) return BadRequest("У вас вже є активне неоплачене замовлення. Дочекайтесь його завершення або дозамовляйте.");

            // === ЛОГІКА РОЗРАХУНКУ З УРАХУВАННЯМ АКЦІЙ ===
            decimal baseTotalAmount = 0;
            var orderItemsFull = new List<(OrderItem item, MenuItem menuItem)>();

            foreach (var item in order.OrderItems)
            {
                var dbMenuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (dbMenuItem != null)
                {
                    item.PriceAtPurchase = dbMenuItem.Price;
                    baseTotalAmount += dbMenuItem.Price * item.Quantity;
                    orderItemsFull.Add((item, dbMenuItem));
                }
            }

            decimal totalDiscount = 0;
            var activePromos = await _context.Promotions.Where(p => p.IsActive).ToListAsync();

            if (activePromos.Any())
            {
                var groupedByCategory = orderItemsFull.GroupBy(x => x.menuItem.CategoryId);

                foreach (var group in groupedByCategory)
                {
                    var promo = activePromos.FirstOrDefault(p => p.CategoryId == null || p.CategoryId == group.Key);
                    if (promo != null)
                    {
                        var flatPrices = new List<decimal>();
                        foreach (var g in group)
                        {
                            for (int i = 0; i < g.item.Quantity; i++) flatPrices.Add(g.item.PriceAtPurchase);
                        }

                        if (promo.PromoType == "1+1=3")
                        {
                            flatPrices = flatPrices.OrderByDescending(p => p).ToList();
                            for (int i = 2; i < flatPrices.Count; i += 3)
                            {
                                totalDiscount += flatPrices[i]; // Кожен 3-й товар безкоштовний
                            }
                        }
                        else if (promo.PromoType == "Discount")
                        {
                            foreach (var price in flatPrices)
                            {
                                totalDiscount += price * (promo.DiscountPercent / 100m);
                            }
                        }
                    }
                }
            }

            decimal calculatedTotalAmount = baseTotalAmount - totalDiscount;
            if (calculatedTotalAmount < 0) calculatedTotalAmount = 0;

            order.TotalAmount = calculatedTotalAmount;
            order.OrderStatus = "Open";
            order.PaymentStatus = "Unpaid";
            order.PaymentMethod = "None";
            order.BonusUsed = 0;
            order.TipAmount = 0;
            order.FinalAmount = calculatedTotalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var savedOrder = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).FirstOrDefaultAsync(o => o.Id == order.Id);
            await SendBaristaNotification(savedOrder, customer.FullName, "НОВЕ ЗАМОВЛЕННЯ", "kitchen");

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPost("{id}/AddItems")]
        public async Task<IActionResult> AddItemsToOrder(int id, [FromBody] List<OrderItem> newItems)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound("Замовлення не знайдено.");
            if (order.PaymentStatus == "Paid") return BadRequest("Замовлення вже оплачено.");

            var customer = await _context.Customers.FindAsync(order.CustomerId);

            foreach (var item in newItems)
            {
                var dbMenuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (dbMenuItem != null)
                {
                    var existingItem = order.OrderItems.FirstOrDefault(oi => oi.MenuItemId == item.MenuItemId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        order.OrderItems.Add(new OrderItem { MenuItemId = item.MenuItemId, Quantity = item.Quantity, PriceAtPurchase = dbMenuItem.Price });
                    }
                }
            }

            // Перерахунок акцій при дозамовленні
            decimal baseTotalAmount = 0;
            var orderItemsFull = new List<(OrderItem item, MenuItem menuItem)>();

            foreach (var item in order.OrderItems)
            {
                var dbMenuItem = await _context.MenuItems.FindAsync(item.MenuItemId);
                if (dbMenuItem != null)
                {
                    baseTotalAmount += item.PriceAtPurchase * item.Quantity;
                    orderItemsFull.Add((item, dbMenuItem));
                }
            }

            decimal totalDiscount = 0;
            var activePromos = await _context.Promotions.Where(p => p.IsActive).ToListAsync();

            if (activePromos.Any())
            {
                var groupedByCategory = orderItemsFull.GroupBy(x => x.menuItem.CategoryId);
                foreach (var group in groupedByCategory)
                {
                    var promo = activePromos.FirstOrDefault(p => p.CategoryId == null || p.CategoryId == group.Key);
                    if (promo != null)
                    {
                        var flatPrices = new List<decimal>();
                        foreach (var g in group)
                        {
                            for (int i = 0; i < g.item.Quantity; i++) flatPrices.Add(g.item.PriceAtPurchase);
                        }

                        if (promo.PromoType == "1+1=3")
                        {
                            flatPrices = flatPrices.OrderByDescending(p => p).ToList();
                            for (int i = 2; i < flatPrices.Count; i += 3) totalDiscount += flatPrices[i];
                        }
                        else if (promo.PromoType == "Discount")
                        {
                            foreach (var price in flatPrices) totalDiscount += price * (promo.DiscountPercent / 100m);
                        }
                    }
                }
            }

            order.TotalAmount = baseTotalAmount - totalDiscount;
            if (order.TotalAmount < 0) order.TotalAmount = 0;
            order.FinalAmount = order.TotalAmount;

            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            await SendBaristaNotification(updatedOrder, customer != null ? customer.FullName : "Гість", "🔄 ДОЗАМОВЛЕННЯ ДО ЧЕКУ", "none");

            return Ok(updatedOrder);
        }

        public class PaymentRequest { public decimal BonusUsed { get; set; } public string PaymentMethod { get; set; } = "Card"; public decimal TipAmount { get; set; } = 0; }

        [HttpPost("{id}/CallWaiter")]
        public async Task<IActionResult> CallWaiter(int id, [FromBody] PaymentRequest request)
        {
            if (request.TipAmount < 0 || request.BonusUsed < 0) return BadRequest("Некоректні дані оплати.");

            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            if (order.PaymentStatus == "Paid") return BadRequest("Вже оплачено.");

            if (order.OrderStatus == "WaitingForPayment") return BadRequest("Ви вже запросили рахунок. Офіціант скоро підійде.");

            var customer = await _context.Customers.FindAsync(order.CustomerId);
            if (customer == null) return BadRequest("Клієнта не знайдено.");

            if (request.BonusUsed > customer.BonusBalance) return BadRequest("Недостатньо бонусів.");
            if (request.BonusUsed > order.TotalAmount) return BadRequest("Не можна списати більше, ніж сума чека.");

            order.TipAmount = request.TipAmount;
            order.BonusUsed = request.BonusUsed;
            order.FinalAmount = order.TotalAmount - order.BonusUsed + order.TipAmount;

            order.PaymentMethod = request.PaymentMethod;
            order.OrderStatus = "WaitingForPayment";

            await _context.SaveChangesAsync();

            string mUa = request.PaymentMethod == "Cash" ? "Готівкою" : "Терміналом";
            await SendBaristaNotification(order, customer.FullName, $"🛎 КЛІЄНТ ПРОСИТЬ РАХУНОК ({mUa})", "payment");

            return Ok(order);
        }

        [HttpPost("{id}/Pay")]
        public async Task<IActionResult> PayOrder(int id, [FromBody] PaymentRequest request)
        {
            if (request.TipAmount < 0 || request.BonusUsed < 0) return BadRequest("Некоректні дані оплати.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound();
                if (order.PaymentStatus == "Paid") return BadRequest("Замовлення вже оплачено.");

                var customer = await _context.Customers.FromSqlRaw("SELECT * FROM \"Customers\" WHERE \"Id\" = {0} FOR UPDATE", order.CustomerId).FirstOrDefaultAsync();
                if (customer == null) return BadRequest("Клієнта не знайдено.");

                if (request.BonusUsed > customer.BonusBalance) return BadRequest("Недостатньо бонусів.");
                if (request.BonusUsed > order.TotalAmount) return BadRequest("Не можна списати більше, ніж сума чека.");

                order.TipAmount = request.TipAmount;
                order.BonusUsed = request.BonusUsed;
                order.FinalAmount = order.TotalAmount - order.BonusUsed + order.TipAmount;

                order.PaymentMethod = request.PaymentMethod;
                order.PaymentStatus = "Paid";
                order.OrderStatus = "Completed";

                if (order.BonusUsed > 0)
                {
                    customer.BonusBalance -= order.BonusUsed;
                    _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, Order = order, PointsDeducted = order.BonusUsed, Reason = "Списання бонусів", CreatedAt = DateTimeOffset.UtcNow });
                }

                if (order.BonusUsed == 0 && customer != null)
                {
                    var pastOrdersCount = await _context.Orders.CountAsync(o => o.CustomerId == customer.Id && o.PaymentStatus == "Paid");
                    decimal cashbackPercent = pastOrdersCount < 15 ? 0.005m : (pastOrdersCount < 50 ? 0.01m : 0.015m);

                    // Перевірка на День Народження (х10 кешбек)
                    bool isBirthday = false;
                    if (customer.DateOfBirth.HasValue)
                    {
                        var today = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(2)); // Час по Києву
                        var dob = customer.DateOfBirth.Value.ToOffset(TimeSpan.FromHours(2));
                        if (today.Day == dob.Day && today.Month == dob.Month)
                        {
                            isBirthday = true;
                            cashbackPercent *= 10;
                        }
                    }

                    decimal pointsEarned = (order.TotalAmount - order.BonusUsed) * cashbackPercent;
                    if (pointsEarned > 0)
                    {
                        customer.BonusBalance += pointsEarned;
                        string reasonText = isBirthday ? "🎁 x10 Кешбек на День Народження!" : "Кешбек за замовлення";
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, Order = order, PointsAdded = pointsEarned, Reason = reasonText, CreatedAt = DateTimeOffset.UtcNow });
                    }
                }

                if (customer.ReferredByCustomerId.HasValue)
                {
                    var referrer = await _context.Customers.FindAsync(customer.ReferredByCustomerId.Value);
                    if (referrer != null)
                    {
                        customer.BonusBalance += 50;
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, PointsAdded = 50, Reason = "Вітальний бонус (Код друга)", CreatedAt = DateTimeOffset.UtcNow });

                        referrer.BonusBalance += 50;
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = referrer.Id, PointsAdded = 50, Reason = $"Бонус за друга ({customer.FullName})", CreatedAt = DateTimeOffset.UtcNow });
                    }
                    customer.ReferredByCustomerId = null;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(order);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Помилка при обробці платежу.");
            }
        }

        [HttpPost("{id}/FakeOnlinePay")]
        public async Task<IActionResult> FakeOnlinePay(int id, [FromBody] PaymentRequest request)
        {
            if (request.TipAmount < 0 || request.BonusUsed < 0) return BadRequest("Некоректні дані оплати.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return NotFound("Замовлення не знайдено.");
                if (order.PaymentStatus == "Paid") return BadRequest("Замовлення вже оплачено.");

                var customer = await _context.Customers.FromSqlRaw("SELECT * FROM \"Customers\" WHERE \"Id\" = {0} FOR UPDATE", order.CustomerId).FirstOrDefaultAsync();
                if (customer == null) return BadRequest("Клієнта не знайдено.");

                if (request.BonusUsed > customer.BonusBalance) return BadRequest("Недостатньо бонусів.");
                if (request.BonusUsed > order.TotalAmount) return BadRequest("Не можна списати більше, ніж сума чека.");

                order.TipAmount = request.TipAmount;
                order.BonusUsed = request.BonusUsed;
                order.FinalAmount = order.TotalAmount - order.BonusUsed + order.TipAmount;

                order.PaymentMethod = "Online (Fake)";
                order.PaymentStatus = "Paid";
                order.OrderStatus = "Completed";

                if (order.BonusUsed > 0 && customer != null)
                {
                    customer.BonusBalance -= order.BonusUsed;
                    _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, Order = order, PointsDeducted = order.BonusUsed, Reason = "Списання бонусів при онлайн оплаті", CreatedAt = DateTimeOffset.UtcNow });
                }

                // 2. Нарахування Кешбеку (тільки якщо не списували бонуси)
                if (order.BonusUsed == 0)
                {
                    var pastOrdersCount = await _context.Orders.CountAsync(o => o.CustomerId == customer.Id && o.PaymentStatus == "Paid");
                    decimal cashbackPercent = pastOrdersCount < 15 ? 0.005m : (pastOrdersCount < 50 ? 0.01m : 0.015m);

                    bool isBirthday = false;
                    if (customer.DateOfBirth.HasValue)
                    {
                        var dobKiev = customer.DateOfBirth.Value.UtcDateTime.AddHours(3);
                        var kievTime = DateTime.UtcNow.AddHours(3);

                        if (dobKiev.Month == kievTime.Month && dobKiev.Day == kievTime.Day)
                        {
                            isBirthday = true;
                            cashbackPercent *= 10;
                        }
                    }

                    decimal points = order.TotalAmount * cashbackPercent;
                    if (points > 0)
                    {
                        customer.BonusBalance += points;
                        string reasonText = isBirthday ? "🎁 x10 Кешбек (З Днем Народження!)" : "Кешбек за замовлення";
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, OrderId = order.Id, PointsAdded = points, Reason = reasonText, CreatedAt = DateTimeOffset.UtcNow });
                    }
                }

                if (customer != null && customer.ReferredByCustomerId.HasValue)
                {
                    var referrer = await _context.Customers.FindAsync(customer.ReferredByCustomerId.Value);
                    if (referrer != null)
                    {
                        customer.BonusBalance += 50;
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, PointsAdded = 50, Reason = "Вітальний бонус (Код друга)", CreatedAt = DateTimeOffset.UtcNow });

                        referrer.BonusBalance += 50;
                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = referrer.Id, PointsAdded = 50, Reason = $"Бонус за друга ({customer.FullName})", CreatedAt = DateTimeOffset.UtcNow });
                    }
                    customer.ReferredByCustomerId = null;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await SendBaristaNotification(order, customer != null ? customer.FullName : "Гість", "✅ ОПЛАЧЕНО ОНЛАЙН (Demo)", "none");

                return Ok(order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Помилка при обробці платежу: {ex.Message}");
            }
        }

        private async Task SendBaristaNotification(Order order, string customerName, string title, string buttonType = "none")
        {
            try
            {
                string botToken = "8701633374:AAG3xQ3UhReoNxnvlz6t101rXBmKtSasbrc";
                string chatId = "-1003907004603";

                var sb = new StringBuilder();
                sb.AppendLine($"🚨 <b>{title} #{order.Id}</b>");
                sb.AppendLine($"👤 Клієнт: {customerName}");
                sb.AppendLine(string.IsNullOrEmpty(order.TableNumber) ? $"📍 На самовиніс" : $"📍 Столик: {order.TableNumber}");
                sb.AppendLine();

                foreach (var item in order.OrderItems)
                {
                    sb.AppendLine($"▫️ {(item.MenuItem != null ? item.MenuItem.Name : "Товар")} x{item.Quantity}");
                }
                sb.AppendLine();

                if (order.BonusUsed > 0) sb.AppendLine($"🎁 Списано бонусів: {order.BonusUsed}");
                if (order.TipAmount > 0) sb.AppendLine($"💸 Чайові: {order.TipAmount} ₴");
                sb.AppendLine($"💰 Разом: {order.FinalAmount} ₴");

                object? replyMarkup = null;

                if (buttonType == "kitchen")
                {
                    replyMarkup = new { inline_keyboard = new[] { new[] { new { text = "👨‍🍳 Взяти в роботу", callback_data = $"process_{order.Id}" }, new { text = "✅ Готово", callback_data = $"ready_{order.Id}" } } } };
                }
                else if (buttonType == "payment")
                {
                    replyMarkup = new { inline_keyboard = new[] { new[] { new { text = "💰 Підтвердити оплату", callback_data = $"pay_{order.Id}" } } } };
                }

                string jsonPayload;
                if (replyMarkup != null)
                {
                    jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, text = sb.ToString(), parse_mode = "HTML", reply_markup = replyMarkup });
                }
                else
                {
                    jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, text = sb.ToString(), parse_mode = "HTML" });
                }

                var jsonOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                string finalJson = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(jsonPayload), jsonOptions);

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync($"https://api.telegram.org/bot{botToken}/sendMessage", new StringContent(finalJson, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПОМИЛКА ВІДПРАВКИ]: {ex.Message}");
            }
        }
    }
}