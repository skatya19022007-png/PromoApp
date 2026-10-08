using Microsoft.EntityFrameworkCore;
using PromoApp.DAL.Interfaces;
using PromoApp.Data;
using PromoApp.Models;

namespace PromoApp.DAL
{
    public class ProductDal : IProductDal
    {
        private readonly AppDbContext _context;

        public ProductDal(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TblProduct>> GetAllAsync()
        {
            return await _context.TblProducts.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<TblProduct?> GetByIdAsync(int id)
        {
            return await _context.TblProducts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> CreateAsync(TblProduct product)
        {
            _context.TblProducts.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> UpdateAsync(TblProduct product)
        {
            var existing = await _context.TblProducts.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.BasePrice = product.BasePrice;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            _context.TblProducts.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}