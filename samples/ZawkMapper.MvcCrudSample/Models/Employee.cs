namespace ZawkMapper.MvcCrudSample.Models;

public sealed class Employee
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public bool IsDeleted { get; set; }
}//Employee
