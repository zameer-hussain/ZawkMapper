using System.ComponentModel.DataAnnotations;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Dtos;

public sealed class CustomerListDto
{
    public long CustomerId { get; set; }
    public Guid PublicId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public int OrdersCount { get; set; }
    public decimal TotalSpent { get; set; }
}

public sealed class CustomerDetailsDto
{
    public long CustomerId { get; set; }
    public Guid PublicId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public List<OrderListDto> Orders { get; set; } = new();
}

public sealed class CustomerCreateDto
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string MobileNumber { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.AddYears(-20);

    public bool IsPremium { get; set; }
}

public sealed class CustomerEditDto
{
    public long CustomerId { get; set; }

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string MobileNumber { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public bool IsPremium { get; set; }
    public CustomerStatus Status { get; set; }
}
