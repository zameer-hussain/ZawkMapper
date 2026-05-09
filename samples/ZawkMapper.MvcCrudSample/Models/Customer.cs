using System.ComponentModel.DataAnnotations;

namespace ZawkMapper.MvcCrudSample.Models;

public sealed class Customer
{
    public long Id { get; set; }
    public Guid PublicId { get; set; } = Guid.NewGuid();

    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string MobileNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public bool IsPremium { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }

    public List<Order> Orders { get; set; } = new();
}
