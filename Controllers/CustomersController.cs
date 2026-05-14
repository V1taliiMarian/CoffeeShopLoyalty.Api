using CoffeeShopLoyalty.Api.Data;
using CoffeeShopLoyalty.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopLoyalty.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GenerateReferralCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("Sync")]
        public async Task<ActionResult<Customer>> SyncCustomer([FromBody] Customer syncData)
        {
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.TelegramId == syncData.TelegramId);

            if (existingCustomer != null)
            {
                if (string.IsNullOrEmpty(existingCustomer.ReferralCode))
                {
                    existingCustomer.ReferralCode = GenerateReferralCode();
                }

                await _context.SaveChangesAsync();
                return Ok(existingCustomer);
            }

            var newCustomer = new Customer
            {
                TelegramId = syncData.TelegramId,
                FullName = syncData.FullName,
                PhoneNumber = syncData.PhoneNumber ?? "Не вказано",
                BonusBalance = 0,
                TotalSpent = 0,
                LoyaltyLevel = "Standard",
                ReferralCode = GenerateReferralCode()
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();
            return Ok(newCustomer);
        }

        public class PhoneRequest { public string PhoneNumber { get; set; } }

        [HttpPost("{id}/UpdatePhone")]
        public async Task<IActionResult> UpdatePhone(int id, [FromBody] PhoneRequest request)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound("Клієнта не знайдено");

            string newPhone = request?.PhoneNumber ?? "";
            if (!string.IsNullOrEmpty(newPhone) && !newPhone.StartsWith("+"))
            {
                newPhone = "+" + newPhone;
            }

            customer.PhoneNumber = newPhone;
            await _context.SaveChangesAsync();
            return Ok(customer);
        }

        public class ReferralRequest { public string Code { get; set; } }

        [HttpPost("{id}/ApplyReferral")]
        public async Task<IActionResult> ApplyReferral(int id, [FromBody] ReferralRequest request)
        {
            var code = request?.Code;
            if (string.IsNullOrWhiteSpace(code)) return BadRequest("Код порожній.");

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound("Клієнта не знайдено");

            if (customer.HasUsedReferral) return BadRequest("Ви вже вводили код від друга раніше.");
            if (customer.ReferralCode == code.ToUpper()) return BadRequest("Не можна ввести власний код.");

            var codeOwner = await _context.Customers.FirstOrDefaultAsync(c => c.ReferralCode == code.ToUpper());
            if (codeOwner == null) return BadRequest("Такого коду не існує.");

            customer.HasUsedReferral = true;
            customer.ReferredByCustomerId = codeOwner.Id;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Код прийнято! Бонуси будуть нараховані після першої оплати.", newBalance = customer.BonusBalance });
        }

        public class ProfileUpdateRequest
        {
            public string FullName { get; set; }
            public DateTimeOffset? DateOfBirth { get; set; }
        }

        [HttpPost("{id}/UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] ProfileUpdateRequest request)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound("Клієнта не знайдено");

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                customer.FullName = request.FullName;
            }

            if (!customer.DateOfBirth.HasValue && request.DateOfBirth.HasValue)
            {
                customer.DateOfBirth = request.DateOfBirth;
            }

            await _context.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            _context.Entry(customer).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!CustomerExists(id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerExists(int id) => _context.Customers.Any(e => e.Id == id);
    }
}