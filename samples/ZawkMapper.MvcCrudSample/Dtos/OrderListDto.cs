namespace ZawkMapper.MvcCrudSample.Dtos;

public sealed class OrderListDto
{
    public long OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDateUtc { get; set; }
}
