using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ZawkMapper.MvcCrudSample.Data;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Customer> Query()
        => _db.Customers.AsNoTracking();

    public Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => _db.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public Task<int> UpdateUsingSqlAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        const string sql = """
        UPDATE Customers
        SET FullName = @FullName,
            Email = @Email,
            MobileNumber = @MobileNumber,
            City = @City,
            DateOfBirth = @DateOfBirth,
            IsPremium = @IsPremium,
            Status = @Status,
            UpdatedAtUtc = @UpdatedAtUtc
        WHERE Id = @Id AND IsDeleted = 0;
        """;

        // SQLite sample uses SqliteParameter. In the main SQL Server project, use SqlParameter with the same pattern.
        var parameters = new object[]
        {
            new SqliteParameter("@FullName", customer.FullName),
            new SqliteParameter("@Email", customer.Email),
            new SqliteParameter("@MobileNumber", customer.MobileNumber),
            new SqliteParameter("@City", customer.City),
            new SqliteParameter("@DateOfBirth", customer.DateOfBirth),
            new SqliteParameter("@IsPremium", customer.IsPremium),
            new SqliteParameter("@Status", (int)customer.Status),
            new SqliteParameter("@UpdatedAtUtc", DateTime.UtcNow),
            new SqliteParameter("@Id", customer.Id)
        };

        return _db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }

    public Task<int> SoftDeleteUsingSqlAsync(long id, CancellationToken cancellationToken = default)
    {
        const string sql = """
        UPDATE Customers
        SET IsDeleted = 1,
            UpdatedAtUtc = @UpdatedAtUtc
        WHERE Id = @Id AND IsDeleted = 0;
        """;

        var parameters = new object[]
        {
            new SqliteParameter("@UpdatedAtUtc", DateTime.UtcNow),
            new SqliteParameter("@Id", id)
        };

        return _db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
    }
}
