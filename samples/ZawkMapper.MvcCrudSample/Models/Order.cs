using System.ComponentModel.DataAnnotations;

namespace ZawkMapper.MvcCrudSample.Models;

public sealed class Order
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [MaxLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
    public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}
