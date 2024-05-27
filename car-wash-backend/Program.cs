using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using car_wash_backend.Api;
using car_wash_backend.Models;
using car_wash_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSpaYarp();

builder.Services.AddDbContext<CarWashContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//будет создан только один сервис при первом запросе
builder.Services.AddScoped<BoxesInCarwashAccessor>();
builder.Services.AddScoped<CarwashAccessor>();
builder.Services.AddScoped<OrderAccessor>();
builder.Services.AddScoped<OrderStatusAccessor>();
builder.Services.AddScoped<PersonAccessor>();
builder.Services.AddScoped<RoleAccessor>();
builder.Services.AddScoped<ServicesAccessor>();
builder.Services.AddScoped<ServiceStatusAccessor>();
builder.Services.AddScoped<UserAccessor>();

builder.Services.AddHttpContextAccessor();//чтобы акцессоры работали
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    using (var dbContext = new CarWashContext())
    {
        var firstUser = await dbContext.Users.FirstOrDefaultAsync(u => u.PersonId.ToString() == "08e85b0b-a800-44a3-ac06-797c94ef7c9a");
        if (firstUser != null)
        {
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, firstUser.UserId.ToString()),
                new Claim(ClaimTypes.UserData, firstUser.RoleId.ToString()),
                new Claim(ClaimTypes.Actor, firstUser.PersonId.ToString())
                // new Claim(ClaimTypes.SerialNumber, firstUser.Login),
                // new Claim(ClaimTypes.UserData, firstUser.Password),
                // new Claim(ClaimTypes.NameIdentifier, firstUser.RoleId.ToString()),
            }));
        }
    }
    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//тут запросы перенаправляются на разработческий сервер (car-wash-service)
//перед развертыванием проекта убрать
app.UseSpaYarp();

app.MapFallbackToFile("index.html");

app.MapGroup("/Carwash").MapCarwashesApi();
app.MapGroup("/Order").MapOrderApi();
app.MapGroup("/Person").MapPersonApi();
app.MapGroup("/Service").MapServicesApi();
app.MapGroup("/User").MapUserApi();
app.MapGroup("/Role").MapRoleApi();

app.Run();