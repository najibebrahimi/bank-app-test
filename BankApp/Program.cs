using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using BankApp.Profiles;
using Services;
using Stripe;
using Stripe.TestHelpers;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BankAppDataContext>();

builder.Services.AddRazorPages();

builder.Services.AddTransient<DataInitializer>();


// Register CustomerService
builder.Services.AddScoped<Services.CustomerService>();

// Add AutoMapper and register the mapping profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddScoped<StatisticsService>();

builder.Services.AddRazorPages();

builder.Services.AddTransient<DataInitializer>();
builder.Services.AddTransient<TransactionService>();
builder.Services.AddTransient<Services.CustomerService>();




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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
