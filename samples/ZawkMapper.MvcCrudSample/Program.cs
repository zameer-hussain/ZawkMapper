using Microsoft.EntityFrameworkCore;
using ZawkMapper.Abstractions;
using ZawkMapper.Configuration;
using ZawkMapper.Core;
using ZawkMapper.MvcCrudSample.Data;
using ZawkMapper.MvcCrudSample.Mapping;
using ZawkMapper.MvcCrudSample.Repositories;
using ZawkMapper.MvcCrudSample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

    if (builder.Configuration.GetValue<bool>("SqlLogging:Enabled"))
    {
        Directory.CreateDirectory("SqlLogs");
        options.EnableSensitiveDataLogging();
        options.LogTo(sql =>
        {
            File.AppendAllText(Path.Combine("SqlLogs", $"sql-{DateTime.UtcNow:yyyyMMdd}.log"), sql + Environment.NewLine + Environment.NewLine);
        });
    }//if sql logging enabled
});

var mapperConfiguration = AppMappingConfig.CreateConfiguration();
builder.Services.AddSingleton(mapperConfiguration);
builder.Services.AddScoped<IObjectMapper, ObjectMapper>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}//using scope

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}//if not development

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customers}/{action=Index}/{id?}");

app.Run();
