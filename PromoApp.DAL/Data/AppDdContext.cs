using Microsoft.EntityFrameworkCore;
using PromoApp.Models;

namespace PromoApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TblRole> TblRoles => Set<TblRole>();
        public DbSet<TblUser> TblUsers => Set<TblUser>();
        public DbSet<TblProduct> TblProducts => Set<TblProduct>();
        public DbSet<TblPromotion> TblPromotions => Set<TblPromotion>();
        public DbSet<TblPromotionItem> TblPromotionItems => Set<TblPromotionItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TblPromotionItem>()
                .HasKey(pi => new { pi.PromotionId, pi.ProductId });

            // 1. Наповнення ролей (20+ записів)
            var roles = new List<TblRole>
            {
                new TblRole { Id = 1, Name = "Менеджер акцій" },
                new TblRole { Id = 2, Name = "Адміністратор" },
                new TblRole { Id = 3, Name = "Касир" },
                new TblRole { Id = 4, Name = "Менеджер з продажу" },
                new TblRole { Id = 5, Name = "Бухгалтер" },
                new TblRole { Id = 6, Name = "Директор" },
                new TblRole { Id = 7, Name = "Товарознавець" },
                new TblRole { Id = 8, Name = "Комірник" },
                new TblRole { Id = 9, Name = "Маркетолог" },
                new TblRole { Id = 10, Name = "Аналітик" },
                new TblRole { Id = 11, Name = "Логіст" },
                new TblRole { Id = 12, Name = "Консультант" },
                new TblRole { Id = 13, Name = "Керівник відділу" },
                new TblRole { Id = 14, Name = "Асистент менеджера" },
                new TblRole { Id = 15, Name = "Оператор бази даних" },
                new TblRole { Id = 16, Name = "Кур'єр" },
                new TblRole { Id = 17, Name = "Контролер якості" },
                new TblRole { Id = 18, Name = "Менеджер повернень" },
                new TblRole { Id = 19, Name = "Охоронець" },
                new TblRole { Id = 20, Name = "Технічний спеціаліст" }
            };
            modelBuilder.Entity<TblRole>().HasData(roles);

            // 2. Наповнення користувачів (20+ записів)
            var users = new List<TblUser>
            {
                new TblUser { Id = 1, Username = "manager", Password = "123", RoleId = 1 },
                new TblUser { Id = 2, Username = "admin", Password = "123", RoleId = 2 },
                new TblUser { Id = 3, Username = "cashier1", Password = "123", RoleId = 3 },
                new TblUser { Id = 4, Username = "sales1", Password = "123", RoleId = 4 },
                new TblUser { Id = 5, Username = "accountant1", Password = "123", RoleId = 5 },
                new TblUser { Id = 6, Username = "director", Password = "123", RoleId = 6 },
                new TblUser { Id = 7, Username = "merchandiser", Password = "123", RoleId = 7 },
                new TblUser { Id = 8, Username = "stockman", Password = "123", RoleId = 8 },
                new TblUser { Id = 9, Username = "marketer", Password = "123", RoleId = 9 },
                new TblUser { Id = 10, Username = "analyst", Password = "123", RoleId = 10 },
                new TblUser { Id = 11, Username = "logistician", Password = "123", RoleId = 11 },
                new TblUser { Id = 12, Username = "consultant1", Password = "123", RoleId = 12 },
                new TblUser { Id = 13, Username = "head_sales", Password = "123", RoleId = 13 },
                new TblUser { Id = 14, Username = "assistant", Password = "123", RoleId = 14 },
                new TblUser { Id = 15, Username = "db_operator", Password = "123", RoleId = 15 },
                new TblUser { Id = 16, Username = "courier1", Password = "123", RoleId = 16 },
                new TblUser { Id = 17, Username = "qc_specialist", Password = "123", RoleId = 17 },
                new TblUser { Id = 18, Username = "returns_manager", Password = "123", RoleId = 18 },
                new TblUser { Id = 19, Username = "security_officer", Password = "123", RoleId = 19 },
                new TblUser { Id = 20, Username = "support_tech", Password = "123", RoleId = 20 }
            };
            modelBuilder.Entity<TblUser>().HasData(users);

            // 3. Наповнення товарів (20+ записів)
            var products = new List<TblProduct>
            {
                new TblProduct { Id = 1, Name = "Ноутбук Dell Vostro", BasePrice = 28000 },
                new TblProduct { Id = 2, Name = "Миша бездротова Logitech", BasePrice = 650 },
                new TblProduct { Id = 3, Name = "Клавіатура механічна HyperX", BasePrice = 2200 },
                new TblProduct { Id = 4, Name = "Монітор 27 Samsung", BasePrice = 9500 },
                new TblProduct { Id = 5, Name = "Навушники Sony WH-1000XM4", BasePrice = 11000 },
                new TblProduct { Id = 6, Name = "Килимок для миші SteelSeries", BasePrice = 450 },
                new TblProduct { Id = 7, Name = "Веб-камера Logitech C920", BasePrice = 3200 },
                new TblProduct { Id = 8, Name = "Акустична система 2.0 Edifier", BasePrice = 4100 },
                new TblProduct { Id = 9, Name = "Флешка Kingston 64GB USB 3.2", BasePrice = 280 },
                new TblProduct { Id = 10, Name = "Зовнішній SSD Samsung 1TB", BasePrice = 3900 },
                new TblProduct { Id = 11, Name = "Маршрутизатор ASUS RT-AX55", BasePrice = 2500 },
                new TblProduct { Id = 12, Name = "Крісло геймерське Hator", BasePrice = 7800 },
                new TblProduct { Id = 13, Name = "Кабель HDMI 2.1 Ultra High Speed", BasePrice = 350 },
                new TblProduct { Id = 14, Name = "USB-хаб Baseus Type-C", BasePrice = 950 },
                new TblProduct { Id = 15, Name = "ДБЖ Eaton 850VA", BasePrice = 4600 },
                new TblProduct { Id = 16, Name = "Підставка для ноутбука DeepCool", BasePrice = 800 },
                new TblProduct { Id = 17, Name = "Мікрофон конденсаторний Fifine", BasePrice = 1900 },
                new TblProduct { Id = 18, Name = "Мережевий фільтр APC 5 розеток", BasePrice = 620 },
                new TblProduct { Id = 19, Name = "Планшет Apple iPad 10.2", BasePrice = 16500 },
                new TblProduct { Id = 20, Name = "Графічний планшет Wacom One", BasePrice = 2900 }
            };
            modelBuilder.Entity<TblProduct>().HasData(products);

            // 4. Наповнення акцій (20+ записів: минулі, активні та майбутні)
            var promotions = new List<TblPromotion>
            {
                new TblPromotion { Id = 1, Title = "Зимовий розпродаж 2025", StartDate = new DateTime(2025, 1, 10), EndDate = new DateTime(2025, 1, 31) },
                new TblPromotion { Id = 2, Title = "Кіберпонеділок 2025", StartDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 2, 7) },
                new TblPromotion { Id = 3, Title = "Весняне оновлення техніки", StartDate = new DateTime(2025, 3, 1), EndDate = new DateTime(2025, 3, 15) },
                new TblPromotion { Id = 4, Title = "Великодній ярмарок", StartDate = new DateTime(2025, 4, 15), EndDate = new DateTime(2025, 4, 25) },
                new TblPromotion { Id = 5, Title = "Травневі знижки", StartDate = new DateTime(2025, 5, 1), EndDate = new DateTime(2025, 5, 10) },
                new TblPromotion { Id = 6, Title = "Літній старт продажів", StartDate = new DateTime(2025, 6, 1), EndDate = new DateTime(2025, 6, 20) },
                new TblPromotion { Id = 7, Title = "Середина літа - гарячі ціни", StartDate = new DateTime(2025, 7, 10), EndDate = new DateTime(2025, 7, 25) },
                new TblPromotion { Id = 8, Title = "Back to School 2025", StartDate = new DateTime(2025, 8, 15), EndDate = new DateTime(2025, 9, 5) },
                new TblPromotion { Id = 9, Title = "Осінній листопад цін", StartDate = new DateTime(2025, 10, 1), EndDate = new DateTime(2025, 10, 20) },
                new TblPromotion { Id = 10, Title = "Чорна П'ятниця 2025", StartDate = new DateTime(2025, 11, 20), EndDate = new DateTime(2025, 11, 30) },
                new TblPromotion { Id = 11, Title = "Новорічний бум 2026", StartDate = new DateTime(2025, 12, 15), EndDate = new DateTime(2026, 1, 5) },
                new TblPromotion { Id = 12, Title = "День програміста 2026", StartDate = new DateTime(2026, 9, 10), EndDate = new DateTime(2026, 9, 20) },
                
                // Активні акції (діють у жовтні 2026)
                new TblPromotion { Id = 13, Title = "Осінній супер-сейл 2026", StartDate = new DateTime(2026, 10, 1), EndDate = new DateTime(2026, 10, 20) },
                new TblPromotion { Id = 14, Title = "Акція на периферію для ПК", StartDate = new DateTime(2026, 10, 5), EndDate = new DateTime(2026, 10, 25) },
                new TblPromotion { Id = 15, Title = "Тиждень брендів Logitech і Dell", StartDate = new DateTime(2026, 10, 7), EndDate = new DateTime(2026, 10, 15) },
                new TblPromotion { Id = 16, Title = "Знижки вихідного дня жовтня", StartDate = new DateTime(2026, 10, 8), EndDate = new DateTime(2026, 10, 12) },

                // Майбутні акції (попереду, можна редагувати)
                new TblPromotion { Id = 17, Title = "Хелловінський розпродаж 2026", StartDate = new DateTime(2026, 10, 28), EndDate = new DateTime(2026, 11, 2) },
                new TblPromotion { Id = 18, Title = "Чорна П'ятниця 2026", StartDate = new DateTime(2026, 11, 20), EndDate = new DateTime(2026, 11, 30) },
                new TblPromotion { Id = 19, Title = "Зимова казка 2027", StartDate = new DateTime(2026, 12, 10), EndDate = new DateTime(2027, 1, 10) },
                new TblPromotion { Id = 20, Title = "Новорічні подарунки 2027", StartDate = new DateTime(2026, 12, 20), EndDate = new DateTime(2027, 1, 15) }
            };
            modelBuilder.Entity<TblPromotion>().HasData(promotions);

            // 5. Наповнення зв'язків між акціями та товарами (20+ записів)
            var promoItems = new List<TblPromotionItem>
            {
                new TblPromotionItem { PromotionId = 1, ProductId = 1, DiscountPercent = 10 },
                new TblPromotionItem { PromotionId = 1, ProductId = 2, DiscountPercent = 15 },
                new TblPromotionItem { PromotionId = 2, ProductId = 3, DiscountPercent = 20 },
                new TblPromotionItem { PromotionId = 3, ProductId = 4, DiscountPercent = 12 },
                new TblPromotionItem { PromotionId = 4, ProductId = 5, DiscountPercent = 18 },
                new TblPromotionItem { PromotionId = 5, ProductId = 6, DiscountPercent = 25 },
                new TblPromotionItem { PromotionId = 6, ProductId = 7, DiscountPercent = 10 },
                new TblPromotionItem { PromotionId = 7, ProductId = 8, DiscountPercent = 15 },
                new TblPromotionItem { PromotionId = 8, ProductId = 9, DiscountPercent = 30 },
                new TblPromotionItem { PromotionId = 9, ProductId = 10, DiscountPercent = 14 },
                new TblPromotionItem { PromotionId = 10, ProductId = 11, DiscountPercent = 22 },
                new TblPromotionItem { PromotionId = 11, ProductId = 12, DiscountPercent = 15 },
                new TblPromotionItem { PromotionId = 12, ProductId = 13, DiscountPercent = 20 },
                new TblPromotionItem { PromotionId = 13, ProductId = 1, DiscountPercent = 8 },
                new TblPromotionItem { PromotionId = 13, ProductId = 4, DiscountPercent = 15 },
                new TblPromotionItem { PromotionId = 14, ProductId = 2, DiscountPercent = 10 },
                new TblPromotionItem { PromotionId = 14, ProductId = 3, DiscountPercent = 12 },
                new TblPromotionItem { PromotionId = 15, ProductId = 1, DiscountPercent = 7 },
                new TblPromotionItem { PromotionId = 16, ProductId = 7, DiscountPercent = 15 },
                new TblPromotionItem { PromotionId = 17, ProductId = 14, DiscountPercent = 20 },
                new TblPromotionItem { PromotionId = 18, ProductId = 5, DiscountPercent = 35 },
                new TblPromotionItem { PromotionId = 19, ProductId = 19, DiscountPercent = 10 },
                new TblPromotionItem { PromotionId = 20, ProductId = 20, DiscountPercent = 18 }
            };
            modelBuilder.Entity<TblPromotionItem>().HasData(promoItems);
        }
    }
}