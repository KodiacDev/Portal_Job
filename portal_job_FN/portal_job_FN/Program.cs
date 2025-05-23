﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;

using Microsoft.EntityFrameworkCore;
using portal_job_FN.Areas.Identity.Pages.Account;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using portal_job_FN.Services.Vnpay;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{

})
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<ILocationRepository, EFLocationRepository>();
builder.Services.AddScoped<IExperienceRepository, EFExperienceRepository>();
builder.Services.AddScoped<IMajorRepository, EFMajorRepository>();
builder.Services.AddScoped<IPostJobRepository, EFPostJobRepository>();
builder.Services.AddScoped<IApplyJobRepository, EFApply_job>();
builder.Services.AddScoped<IUniversity, EFUniversityRepository>();
builder.Services.AddScoped<IEducationRepository, EFEducationRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IVnPayRepository, EFVnPayRepository>();
//Vnpay
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LogoutPath = $"/Identity/Account/AccessDenied";
});


builder.Services.AddControllers();
builder.Services.AddRazorPages();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // Set the max file size (100 MB in this case)
});



//Cau hình session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình Google authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var gconfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = gconfig["ClientId"];
        options.ClientSecret = gconfig["ClientSecret"];
        options.CallbackPath = "/signin-google"; // hoặc "/dang-nhap-tu-google" tùy bạn định nghĩa
    });


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

//dat truoc use routing
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "Company",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "User",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
