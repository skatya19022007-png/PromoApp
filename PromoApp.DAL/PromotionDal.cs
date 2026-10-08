using Microsoft.EntityFrameworkCore;
using PromoApp.DAL.Interfaces;
using PromoApp.Data;
using PromoApp.Models;

namespace PromoApp.DAL
{
    public class PromotionDal : IPromotionDal
    {
        private readonly AppDbContext _context;

        public PromotionDal(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TblPromotion>> GetAllAsync(string status = "all")
        {
            var today = DateTime.Today;
            var query = _context.TblPromotions.Include(p => p.Items).ThenInclude(i => i.Product).AsQueryable();

            if (status == "past") query = query.Where(p => p.EndDate < today);
            else if (status == "active") query = query.Where(p => p.StartDate <= today && p.EndDate >= today);
            else if (status == "future") query = query.Where(p => p.StartDate > today);

            return await query.OrderByDescending(p => p.StartDate).ToListAsync();
        }

        public async Task<TblPromotion?> GetByIdAsync(int id)
        {
            return await _context.TblPromotions
                .Include(p => p.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> CreateAsync(TblPromotion promo, List<(int productId, decimal discount)> items)
        {
            _context.TblPromotions.Add(promo);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                _context.TblPromotionItems.Add(new TblPromotionItem
                {
                    PromotionId = promo.Id,
                    ProductId = item.productId,
                    DiscountPercent = item.discount
                });
            }

            await _context.SaveChangesAsync();
            return promo.Id;
        }

        public async Task<bool> UpdateAsync(TblPromotion promo, List<(int productId, decimal discount)> items)
        {
            var existing = await _context.TblPromotions
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == promo.Id);

            if (existing == null) return false;

            existing.Title = promo.Title;
            existing.StartDate = promo.StartDate;
            existing.EndDate = promo.EndDate;

            _context.TblPromotionItems.RemoveRange(existing.Items);

            foreach (var item in items)
            {
                existing.Items.Add(new TblPromotionItem
                {
                    PromotionId = existing.Id,
                    ProductId = item.productId,
                    DiscountPercent = item.discount
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var promo = await _context.TblPromotions
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (promo == null) return false;

            _context.TblPromotionItems.RemoveRange(promo.Items);
            _context.TblPromotions.Remove(promo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}