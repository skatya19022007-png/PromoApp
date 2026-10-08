using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromoApp.Data;
using PromoApp.Models;

namespace PromoApp.Controllers
{
    [Authorize(Roles = "Менеджер акцій")]
    public class PromotionsController : Controller
    {
        private readonly AppDbContext _context;

        public PromotionsController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(string status = "all")
        {
            var today = DateTime.Today;
            var query = _context.TblPromotions.AsQueryable();

            if (status == "past") query = query.Where(p => p.EndDate < today);
            else if (status == "active") query = query.Where(p => p.StartDate <= today && p.EndDate >= today);
            else if (status == "future") query = query.Where(p => p.StartDate > today);

            var list = await query.OrderByDescending(p => p.StartDate).ToListAsync();
            ViewBag.CurrentFilter = status;
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.TblProducts.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TblPromotion promo, int[] productIds, decimal[] discounts)
        {
            _context.TblPromotions.Add(promo);
            await _context.SaveChangesAsync();

            for (int i = 0; i < productIds.Length; i++)
            {
                if (productIds[i] > 0 && discounts[i] > 0)
                {
                    _context.TblPromotionItems.Add(new TblPromotionItem
                    {
                        PromotionId = promo.Id,
                        ProductId = productIds[i],
                        DiscountPercent = discounts[i]
                    });
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var promo = await _context.TblPromotions
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promo == null) return NotFound();

            if (promo.Status != "майбутня")
                return BadRequest("Редагувати можна лише майбутні акції!");

            ViewBag.Products = await _context.TblProducts.ToListAsync();
            return View(promo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TblPromotion updated, int[] productIds, decimal[] discounts)
        {
            var promo = await _context.TblPromotions
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promo == null) return NotFound();

            if (promo.Status != "майбутня")
                return BadRequest("Редагувати можна лише майбутні акції!");

            promo.Title = updated.Title;
            promo.StartDate = updated.StartDate;
            promo.EndDate = updated.EndDate;

            _context.TblPromotionItems.RemoveRange(promo.Items);

            for (int i = 0; i < productIds.Length; i++)
            {
                if (productIds[i] > 0 && discounts[i] > 0)
                {
                    promo.Items.Add(new TblPromotionItem
                    {
                        PromotionId = promo.Id,
                        ProductId = productIds[i],
                        DiscountPercent = discounts[i]
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}