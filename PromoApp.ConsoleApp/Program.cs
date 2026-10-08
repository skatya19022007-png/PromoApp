using Microsoft.EntityFrameworkCore;
using PromoApp.DAL;
using PromoApp.DAL.Interfaces;
using PromoApp.Data;
using PromoApp.Models;
using System.Text;

// Налаштування для коректного відображення українських літер у консолі
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Отримуємо рядок підключення до бази (такий самий, як у Web-проєкті)
var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PromoTradeDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

using var context = new AppDbContext(optionsBuilder.Options);

// Ініціалізуємо DAL-класи
IProductDal productDal = new ProductDal(context);
IPromotionDal promoDal = new PromotionDal(context);

while (true)
{
    Console.WriteLine("\n==============================================");
    Console.WriteLine("        ТОРГОВЕ ПІДПРИЄМСТВО (DAL CRUD)       ");
    Console.WriteLine("==============================================");
    Console.WriteLine("1. Показати всі товари (Read)");
    Console.WriteLine("2. Додати новий товар (Create)");
    Console.WriteLine("3. Оновити товар (Update)");
    Console.WriteLine("4. Видалити товар (Delete)");
    Console.WriteLine("----------------------------------------------");
    Console.WriteLine("5. Показати всі акції (Read)");
    Console.WriteLine("6. Створити акцію (Create)");
    Console.WriteLine("7. Видалити акцію (Delete)");
    Console.WriteLine("0. Вихід");
    Console.WriteLine("==============================================");
    Console.Write("Оберіть опцію: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            var products = await productDal.GetAllAsync();
            Console.WriteLine("\n--- СПИСОК ТОВАРІВ ---");
            foreach (var p in products)
            {
                Console.WriteLine($"ID: {p.Id,-3} | {p.Name,-35} | {p.BasePrice,8} грн");
            }
            break;

        case "2":
            Console.Write("\nВведіть назву товару: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Введіть базову ціну: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                var newId = await productDal.CreateAsync(new TblProduct { Name = name, BasePrice = price });
                Console.WriteLine($"[Успіх] Товар створено з ID = {newId}");
            }
            else
            {
                Console.WriteLine("[Помилка] Некоректна ціна!");
            }
            break;

        case "3":
            Console.Write("\nВведіть ID товару для оновлення: ");
            if (int.TryParse(Console.ReadLine(), out int updateId))
            {
                var prod = await productDal.GetByIdAsync(updateId);
                if (prod != null)
                {
                    Console.Write($"Нова назва (було '{prod.Name}'): ");
                    var newTitle = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newTitle)) prod.Name = newTitle;

                    Console.Write($"Нова ціна (було {prod.BasePrice}): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newP)) prod.BasePrice = newP;

                    var success = await productDal.UpdateAsync(prod);
                    Console.WriteLine(success ? "[Успіх] Товар оновлено!" : "[Помилка] Не вдалося оновити.");
                }
                else
                {
                    Console.WriteLine("[Помилка] Товар з таким ID не знайдено.");
                }
            }
            break;

        case "4":
            Console.Write("\nВведіть ID товару для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int delId))
            {
                var res = await productDal.DeleteAsync(delId);
                Console.WriteLine(res ? "[Успіх] Товар видалено." : "[Помилка] Товар не знайдено.");
            }
            break;

        case "5":
            var promos = await promoDal.GetAllAsync("all");
            Console.WriteLine("\n--- СПИСОК АКЦІЙ ---");
            foreach (var pr in promos)
            {
                Console.WriteLine($"ID: {pr.Id,-3} | {pr.Title,-30} | {pr.StartDate:dd.MM.yyyy} - {pr.EndDate:dd.MM.yyyy} | Статус: {pr.Status}");
            }
            break;

        case "6":
            Console.Write("\nВведіть назву акції: ");
            var pTitle = Console.ReadLine() ?? "";
            Console.Write("Дата початку (рррр-мм-дд): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime sDate);
            Console.Write("Дата кінця (рррр-мм-дд): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime eDate);

            var promoToCreate = new TblPromotion { Title = pTitle, StartDate = sDate, EndDate = eDate };
            var items = new List<(int, decimal)>();

            Console.Write("Додати товар до акції? Введіть ID товару (або 0 для пропуску): ");
            if (int.TryParse(Console.ReadLine(), out int pId) && pId > 0)
            {
                Console.Write("Знижка (%): ");
                decimal.TryParse(Console.ReadLine(), out decimal disc);
                items.Add((pId, disc));
            }

            var createdPromoId = await promoDal.CreateAsync(promoToCreate, items);
            Console.WriteLine($"[Успіх] Акцію створено з ID = {createdPromoId}");
            break;

        case "7":
            Console.Write("\nВведіть ID акції для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int delPromoId))
            {
                var deleted = await promoDal.DeleteAsync(delPromoId);
                Console.WriteLine(deleted ? "[Успіх] Акцію видалено." : "[Помилка] Акцію не знайдено.");
            }
            break;

        case "0":
            Console.WriteLine("Завершення роботи.");
            return;

        default:
            Console.WriteLine("Невідома команда. Спробуйте ще раз.");
            break;
    }
}