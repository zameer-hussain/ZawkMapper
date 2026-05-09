using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (db.Customers.Any())
            return;

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
        await db.SaveChangesAsync();
    }//SeedAsync
}//DbSeeder
