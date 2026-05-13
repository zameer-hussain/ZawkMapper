using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!db.Customers.Any())
        {
            var cities = new[] { "Karachi", "Hyderabad", "Lahore", "Islamabad", "Sukkur", "Multan", "Peshawar", "Quetta" };
            var customers = new List<Customer>();

            for (var i = 1; i <= 5000; i++)
            {
                var customer = new Customer
                {
                    FullName = $"Customer {i:0000}",
                    Email = $"customer{i:0000}@example.com",
                    MobileNumber = $"0300{i:0000000}",
                    City = cities[i % cities.Length],
                    DateOfBirth = DateTime.UtcNow.Date.AddYears(-18 - (i % 35)).AddDays(-(i % 365)),
                    IsPremium = i % 4 == 0,
                    Status = i % 15 == 0 ? CustomerStatus.Suspended : (i % 7 == 0 ? CustomerStatus.Inactive : CustomerStatus.Active),
                    CreatedAtUtc = DateTime.UtcNow.AddDays(-(i % 365))
                };

                for (var j = 1; j <= 5; j++)
                {
                    customer.Orders.Add(new Order
                    {
                        OrderNumber = $"ORD-{i:0000}-{j:00}",
                        TotalAmount = 500 + (i % 50) * 25 + j * 100,
                        OrderDateUtc = DateTime.UtcNow.AddDays(-((i + j) % 180)),
                        IsDeleted = j == 5 && i % 3 == 0
                    });
                }//for order

                customers.Add(customer);
            }//for customer

            db.Customers.AddRange(customers);
        }//if customers

        if (!db.Products.Any())
        {
            db.Products.AddRange(new[]
            {
                new Product
                {
                    Code = "PRD-001",
                    NameEn = "Sindhi Ajrak",
                    NameSd = "سنڌي اجرڪ",
                    DescriptionEn = "A cultural Ajrak design used in the ZawkMapper localization sample.",
                    DescriptionSd = "ZawkMapper لوڪلائيزيشن نموني لاءِ ثقافتي اجرڪ ڊيزائن.",
                    Price = 2500m
                },
                new Product
                {
                    Code = "PRD-002",
                    NameEn = "Sindhi Topi",
                    NameSd = "سنڌي ٽوپي",
                    DescriptionEn = "A simple product row used to switch English and Sindhi fields.",
                    DescriptionSd = "انگريزي ۽ سنڌي فيلڊ تبديل ڪرڻ لاءِ سادي پراڊڪٽ قطار.",
                    Price = 1800m
                },
                new Product
                {
                    Code = "PRD-003",
                    NameEn = "Learning Notebook",
                    NameSd = "سکيا واري ڪاپي",
                    DescriptionEn = "This row is used for prefix and suffix projection examples.",
                    DescriptionSd = "هي قطار prefix ۽ suffix projection مثالن لاءِ استعمال ٿئي ٿي.",
                    Price = 450m
                }
            });
        }//if products

        if (!db.Employees.Any())
        {
            db.Employees.AddRange(new[]
            {
                new Employee { FullName = "Ali Ahmed", Department = "Engineering", Salary = 100000m },
                new Employee { FullName = "Zahid Hussain", Department = "Support", Salary = 75000m },
                new Employee { FullName = "Sana Memon", Department = "Design", Salary = 90000m }
            });
        }//if employees

        await db.SaveChangesAsync();
    }//SeedAsync
}//DbSeeder
