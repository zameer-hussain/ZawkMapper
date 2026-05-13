namespace ZawkMapper.MvcCrudSample.Dtos;

public sealed class EmployeeSalaryDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal CurrentSalary { get; set; }
    public decimal ProjectedSalary { get; set; }
}//EmployeeSalaryDto
