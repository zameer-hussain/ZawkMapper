using ZawkMapper.Configuration;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Mapping;

public static class ProductScenarioConfig
{
    public static MapperConfiguration CreatePrefixSuffixProjectionConfig(string prefix, string suffix)
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.ProjectModel<Product, ProductPrefixSuffixDto>()
                .MapFieldStrict(dest => dest.Id, src => src.Id)
                .MapField(dest => dest.DisplayName, src => prefix + src.NameEn + suffix);
        });
    }//CreatePrefixSuffixProjectionConfig
}//ProductScenarioConfig
