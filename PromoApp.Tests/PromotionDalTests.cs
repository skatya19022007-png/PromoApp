using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PromoApp.DAL;
using PromoApp.Data;
using PromoApp.Models;

namespace PromoApp.Tests
{
    public class PromotionDalTests
    {
        [Test]
        public async Task GetAll_ReturnsAllPromotions() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.TblPromotions.Add(new TblPromotion
                {
                    Id = 1,
                    Title = "Тестова акція 1",
                    StartDate = DateTime.Today.AddDays(-5),
                    EndDate = DateTime.Today.AddDays(5)
                });
                context.TblPromotions.Add(new TblPromotion
                {
                    Id = 2,
                    Title = "Тестова акція 2",
                    StartDate = DateTime.Today.AddDays(10),
                    EndDate = DateTime.Today.AddDays(20)
                });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var dal = new PromotionDal(context);
                var list = await dal.GetAllAsync("all");

                Assert.That(list.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task Create_AddsPromotionWithItem() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var dal = new PromotionDal(context);

            var promo = new TblPromotion
            {
                Title = "Знижки на мишки",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7)
            };

            var items = new List<(int productId, decimal discount)>
            {
                (productId: 1, discount: 15)
            };

            var newId = await dal.CreateAsync(promo, items);
            var created = await dal.GetByIdAsync(newId);

            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Title, Is.EqualTo("Знижки на мишки"));
            Assert.That(created.Items.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Delete_RemovesPromotionSuccessfully() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.TblPromotions.Add(new TblPromotion
                {
                    Id = 10,
                    Title = "Акція для видалення",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1)
                });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var dal = new PromotionDal(context);
                var deleted = await dal.DeleteAsync(10);
                var promo = await dal.GetByIdAsync(10);

                Assert.That(deleted, Is.True);
                Assert.That(promo, Is.Null);
            }
        }
    }
}