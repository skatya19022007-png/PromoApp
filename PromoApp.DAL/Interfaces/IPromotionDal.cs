using PromoApp.Models;

namespace PromoApp.DAL.Interfaces
{
    public interface IPromotionDal
    {
        Task<List<TblPromotion>> GetAllAsync(string status = "all");
        Task<TblPromotion?> GetByIdAsync(int id);
        Task<int> CreateAsync(TblPromotion promo, List<(int productId, decimal discount)> items);
        Task<bool> UpdateAsync(TblPromotion promo, List<(int productId, decimal discount)> items);
        Task<bool> DeleteAsync(int id);
    }
}