using System.Reflection;
using ZawkMapper.Configuration;

namespace ZawkMapper.MvcCrudSample.Mapping;

public static class AppMappingConfig
{
    private static readonly Lazy<MapperConfiguration> CachedConfiguration = new(CreateConfigurationInternal);

    public static MapperConfiguration CreateConfiguration()
    {
        return CachedConfiguration.Value;
    }//CreateConfiguration

    public static MapperConfiguration StaticConfigMethod()
    {
        return CachedConfiguration.Value;
    }//StaticConfigMethod

    private static MapperConfiguration CreateConfigurationInternal()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfilesFromAssembly(Assembly.GetExecutingAssembly());
        });

        config.AssertConfigurationIsValid();
        return config;
    }//CreateConfigurationInternal
}//AppMappingConfig
