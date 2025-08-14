using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankApp.DataAccessLayer.Models;
using BankApp.Profiles;
using BankAppServices = BankApp.Services;
using Stripe;
using Stripe.TestHelpers;
using BankApp.Services.ViewModels;
using BankApp.DataAccessLayer;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BankAppDataContext>();

builder.Services.AddRazorPages();

builder.Services.AddTransient<DataInitializer>();

// Add AutoMapper and register the mapping profile
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddRazorPages();

builder.Services.AddTransient<DataInitializer>();

builder.Services.AddTransient<BankAppServices.IStatisticsService, BankAppServices.StatisticsService>();
builder.Services.AddTransient<BankAppServices.ITransactionService, BankAppServices.TransactionService>();
builder.Services.AddTransient<BankAppServices.IAccountService, BankAppServices.AccountService>();
builder.Services.AddTransient<BankAppServices.ICustomerService, BankAppServices.CustomerService>();

builder.Services.AddTransient<StatisticsViewModel>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitializer>().SeedData();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
