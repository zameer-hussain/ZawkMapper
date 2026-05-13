namespace ZawkMapper.MvcCrudSample.Dtos;

public sealed class ProductLocalizedDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CultureLabel { get; set; } = string.Empty;
}//ProductLocalizedDto

public sealed class ProductRuntimeProjectionDto
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}//ProductRuntimeProjectionDto

public sealed class ProductPrefixSuffixDto
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}//ProductPrefixSuffixDto

public sealed class RuntimeProjectionCompareDto
{
    public ProductRuntimeProjectionDto RuntimeMapped { get; set; } = new();
    public ProductRuntimeProjectionDto Projected { get; set; } = new();
}//RuntimeProjectionCompareDto
