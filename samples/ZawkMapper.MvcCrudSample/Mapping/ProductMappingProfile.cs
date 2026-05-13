using ZawkMapper.Configuration;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Mapping;

public sealed class ProductMappingProfile : MappingProfile
{
    public override void Configure(MapperConfiguration cfg)
    {
        cfg.MapModel<Product, ProductRuntimeProjectionDto>()
            .MapFieldStrict(dest => dest.Id, src => src.Id)
            .MapField(dest => dest.DisplayName, src => FormatForRuntime(src.Code, src.NameEn));

        cfg.ProjectModel<Product, ProductRuntimeProjectionDto>()
            .MapFieldStrict(dest => dest.Id, src => src.Id)
            .MapField(dest => dest.DisplayName, src => src.Code + " - " + src.NameEn);

        cfg.ProjectModel<Product, ProductLocalizedDto>(ProductProjectionNames.English)
            .MapFieldStrict(dest => dest.Id, src => src.Id)
            .MapFieldStrict(dest => dest.Code, src => src.Code)
            .MapField(dest => dest.Name, src => src.NameEn)
            .MapField(dest => dest.Description, src => src.DescriptionEn)
            .MapFieldStrict(dest => dest.Price, src => src.Price)
            .MapField(dest => dest.CultureLabel, src => "English");

        cfg.ProjectModel<Product, ProductLocalizedDto>(ProductProjectionNames.Sindhi)
            .MapFieldStrict(dest => dest.Id, src => src.Id)
            .MapFieldStrict(dest => dest.Code, src => src.Code)
            .MapField(dest => dest.Name, src => src.NameSd)
            .MapField(dest => dest.Description, src => src.DescriptionSd)
            .MapFieldStrict(dest => dest.Price, src => src.Price)
            .MapField(dest => dest.CultureLabel, src => "Sindhi");
    }//Configure

    private static string FormatForRuntime(string code, string name)
    {
        return $"runtime method: {code} | {name}";
    }//FormatForRuntime
}//ProductMappingProfile
