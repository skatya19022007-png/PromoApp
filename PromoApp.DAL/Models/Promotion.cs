using System.ComponentModel.DataAnnotations.Schema;

namespace PromoApp.Models
{
    public class TblPromotion
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TblPromotionItem> Items { get; set; } = new();

        [NotMapped]
        public string Status
        {
            get
            {
                var today = DateTime.Today;
                if (EndDate.Date < today) return "минула";
                if (StartDate.Date <= today && EndDate.Date >= today) return "активна";
                return "майбутня";
            }
        }
    }

    public class TblPromotionItem
    {
        public int PromotionId { get; set; }
        public TblPromotion? Promotion { get; set; }

        public int ProductId { get; set; }
        public TblProduct? Product { get; set; }

        public decimal DiscountPercent { get; set; }
    }
}