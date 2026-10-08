using PromoApp.Models;

namespace PromoApp.DAL.Interfaces
{
    public interface IProductDal
    {
        Task<List<TblProduct>> GetAllAsync();
        Task<TblProduct?> GetByIdAsync(int id);
        Task<int> CreateAsync(TblProduct product);
        Task<bool> UpdateAsync(TblProduct product);
        Task<bool> DeleteAsync(int id);
    }
}