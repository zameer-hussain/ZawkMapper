using ZawkMapper.Configuration;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Mapping;

public static class EmployeeScenarioConfig
{
    public static MapperConfiguration CreateSalaryProjectionConfig(decimal incrementPercent)
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.ProjectModel<Employee, EmployeeSalaryDto>()
                .MapFieldStrict(dest => dest.Id, src => src.Id)
                .MapFieldStrict(dest => dest.FullName, src => src.FullName)
                .MapFieldStrict(dest => dest.Department, src => src.Department)
                .MapField(dest => dest.CurrentSalary, src => src.Salary)
                .MapField(dest => dest.ProjectedSalary, src => src.Salary + (src.Salary * incrementPercent / 100));
        });
    }//CreateSalaryProjectionConfig
}//EmployeeScenarioConfig
