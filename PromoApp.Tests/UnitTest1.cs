using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PromoApp.DAL;
using PromoApp.Data;
using PromoApp.Models;

namespace PromoApp.Tests
{
    public class ProductDalTests
    {
        [Test]
        public async Task GetAll_ReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.TblProducts.Add(new TblProduct { Id = 101, Name = "Тестовий товар 1", BasePrice = 100 });
                context.TblProducts.Add(new TblProduct { Id = 102, Name = "Тестовий товар 2", BasePrice = 200 });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var dal = new ProductDal(context);
                var result = await dal.GetAllAsync();

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result[0].Name, Is.EqualTo("Тестовий товар 1"));
            }
        }

        [Test]
        public async Task Create_AddsProductSuccessfully()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var dal = new ProductDal(context);

            var newId = await dal.CreateAsync(new TblProduct { Name = "Новий товар", BasePrice = 500 });
            var created = await dal.GetByIdAsync(newId);

            Assert.That(created, Is.Not.Null);
            Assert.That(created!.Name, Is.EqualTo("Новий товар"));
        }

        [Test]
        public async Task Delete_RemovesProductSuccessfully()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.TblProducts.Add(new TblProduct { Id = 201, Name = "Товар для видалення", BasePrice = 300 });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var dal = new ProductDal(context);
                var deleted = await dal.DeleteAsync(201);
                var product = await dal.GetByIdAsync(201);

                Assert.That(deleted, Is.True);
                Assert.That(product, Is.Null);
            }
        }
    }
}