using MealMake.Domain.Identity;
using MealMake.Repository.Data;
using MealMake.Repository.Implementation;
using MealMake.Repository.Interface;
using MealMake.Service.Implementation;
using MealMake.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging(false) // optional: hides parameter values
           .LogTo(_ => { }); // prevent EF Core from printing SQL/logs
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddDefaultIdentity<MealMakeApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddHttpClient<IDataFetchService, DataFetchService>();
builder.Services.AddTransient<IMealService, MealService>();
builder.Services.AddTransient<IMealCollectionService, MealCollectionService>();
builder.Services.AddTransient<ICollectionCategoryService, CollectionCategoryService>();
builder.Services.AddTransient<IActiveCollectionService, ActiveCollectionService>();
builder.Services.AddTransient<IArchivedCollectionService, ArchivedCollectionService>();
builder.Services.AddTransient<IOpenAIService, OpenAIService>();








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MealCollections}/{action=HomePage}/{id?}");
app.MapRazorPages();



app.Run();
