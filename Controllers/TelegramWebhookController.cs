using CoffeeShopLoyalty.Api.Data;
using CoffeeShopLoyalty.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoffeeShopLoyalty.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramWebhookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _botToken = "8701633374:AAG3xQ3UhReoNxnvlz6t101rXBmKtSasbrc";
        public TelegramWebhookController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveUpdate()
        {
            try
            {
                using var document = await JsonDocument.ParseAsync(Request.Body);
                var root = document.RootElement;

                // 1. ОБРОБКА КОНТАКТУ
                if (root.TryGetProperty("message", out var message) && message.TryGetProperty("contact", out var contact))
                {
                    if (message.TryGetProperty("from", out var from) && from.TryGetProperty("id", out var tgIdElement))
                    {
                        long telegramId = tgIdElement.GetInt64();
                        string phoneNumber = contact.GetProperty("phone_number").GetString() ?? "";

                        // ДОДАЄМО ПЛЮС, ЯКЩО ЙОГО НЕМАЄ
                        if (!string.IsNullOrEmpty(phoneNumber) && !phoneNumber.StartsWith("+"))
                        {
                            phoneNumber = "+" + phoneNumber;
                        }

                        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.TelegramId == telegramId);
                        if (customer != null)
                        {
                            customer.PhoneNumber = phoneNumber;
                            await _context.SaveChangesAsync();
                            await SendTelegramMessage(telegramId, "✅ Ваш номер телефону успішно збережено! Тепер ви можете повернутися до додатку.");
                        }
                    }
                    return Ok();
                }

                // 2. ОБРОБКА КНОПОК БАРИСТИ
                if (root.TryGetProperty("callback_query", out var callbackQuery))
                {
                    string callbackId = callbackQuery.GetProperty("id").GetString() ?? "";
                    string data = callbackQuery.GetProperty("data").GetString() ?? "";

                    var cbMessage = callbackQuery.GetProperty("message");
                    long chatId = cbMessage.GetProperty("chat").GetProperty("id").GetInt64();
                    long messageId = cbMessage.GetProperty("message_id").GetInt64();
                    string oldText = cbMessage.GetProperty("text").GetString() ?? "";

                    var parts = data.Split('_');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int orderId))
                    {
                        var order = await _context.Orders.FindAsync(orderId);
                        if (order != null)
                        {
                            string newStatusText = "";

                            if (parts[0] == "process")
                            {
                                order.OrderStatus = "Processing";
                                newStatusText = "👨‍🍳 Готується";
                                var replyMarkup = new { inline_keyboard = new[] { new[] { new { text = "✅ Готово", callback_data = $"ready_{orderId}" } } } };
                                await EditTelegramMessage(chatId, messageId, $"{oldText}\n\n<b>Статус:</b> {newStatusText}", replyMarkup);
                            }
                            else if (parts[0] == "ready")
                            {
                                order.OrderStatus = "Ready";
                                newStatusText = "✅ Готово до видачі";
                                await EditTelegramMessage(chatId, messageId, $"{oldText}\n\n<b>Статус:</b> {newStatusText}", null);
                            }
                            else if (parts[0] == "pay")
                            {
                                if (order.PaymentStatus != "Paid")
                                {
                                    var customer = await _context.Customers.FindAsync(order.CustomerId);
                                    order.PaymentStatus = "Paid";
                                    order.OrderStatus = "Completed";

                                    if (order.BonusUsed > 0 && customer != null)
                                    {
                                        customer.BonusBalance -= order.BonusUsed;
                                        _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, Order = order, PointsDeducted = order.BonusUsed, Reason = "Списання бонусів", CreatedAt = DateTimeOffset.UtcNow });
                                    }

                                    if (order.BonusUsed == 0 && customer != null)
                                    {
                                        var pastOrdersCount = await _context.Orders.CountAsync(o => o.CustomerId == customer.Id && o.PaymentStatus == "Paid");
                                        decimal cashbackPercent = pastOrdersCount < 15 ? 0.005m : (pastOrdersCount < 50 ? 0.01m : 0.015m);
                                        decimal pointsEarned = (order.TotalAmount - order.BonusUsed) * cashbackPercent;
                                        if (pointsEarned > 0)
                                        {
                                            customer.BonusBalance += pointsEarned;
                                            _context.LoyaltyTransactions.Add(new LoyaltyTransaction { CustomerId = customer.Id, Order = order, PointsAdded = pointsEarned, Reason = "Кешбек", CreatedAt = DateTimeOffset.UtcNow });
                                        }
                                    }

                                    newStatusText = "✅ Оплату підтверджено";
                                    await EditTelegramMessage(chatId, messageId, $"{oldText}\n\n<b>Статус:</b> {newStatusText}", null);
                                }
                            }

                            await _context.SaveChangesAsync();
                        }
                    }

                    await AnswerCallbackQuery(callbackId);
                    return Ok();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook Error: {ex.Message}");
                return Ok();
            }
        }

        private async Task SendTelegramMessage(long chatId, string text)
        {
            var jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, text = text, parse_mode = "HTML" });
            using var httpClient = new HttpClient();
            await httpClient.PostAsync($"https://api.telegram.org/bot{_botToken}/sendMessage", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
        }

        private async Task EditTelegramMessage(long chatId, long messageId, string text, object? replyMarkup)
        {
            string jsonPayload;
            if (replyMarkup != null)
            {
                jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, message_id = messageId, text = text, parse_mode = "HTML", reply_markup = replyMarkup });
            }
            else
            {
                jsonPayload = JsonSerializer.Serialize(new { chat_id = chatId, message_id = messageId, text = text, parse_mode = "HTML", reply_markup = new { inline_keyboard = Array.Empty<object>() } });
            }

            using var httpClient = new HttpClient();
            await httpClient.PostAsync($"https://api.telegram.org/bot{_botToken}/editMessageText", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
        }

        private async Task AnswerCallbackQuery(string callbackQueryId)
        {
            var jsonPayload = JsonSerializer.Serialize(new { callback_query_id = callbackQueryId });
            using var httpClient = new HttpClient();
            await httpClient.PostAsync($"https://api.telegram.org/bot{_botToken}/answerCallbackQuery", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
        }
    }
}