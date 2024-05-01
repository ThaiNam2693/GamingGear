using AutoMapper;
using DataAccess.DataAccess;
using DataAccess.IRepository;
using DataAccess.Repository;
using SWP391_Group3_FinalProject.Filter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region AutoMapper
builder.Services.AddSingleton<IMapper>(sp => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()))));
#endregion
//-----------------------------------------------------------------
#region Singleton
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
#endregion

//-----------------------------------------------------------------
#region Session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
});

#endregion

builder.Services.AddScoped<LoginFilter>();
builder.Services.AddScoped<CheckOutFilter>();
builder.Services.AddScoped<CustomerFilter>();

var app = builder.Build();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePagesWithReExecute("/StatusCodeError/{0}");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
