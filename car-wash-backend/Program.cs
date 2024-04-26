using System.Security.Claims;
using car_wash_backend.Api;
using car_wash_backend.Models;
using car_wash_backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSpaYarp();

builder.Services.AddDbContext<CarWashContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//будет создан только один сервис при первом запросе
builder.Services.AddScoped<UserAccessor>();
builder.Services.AddScoped<CarwashAccessor>();

builder.Services.AddHttpContextAccessor();//чтобы акцессоры работали
builder.Services.AddHttpClient();

var app = builder.Build();

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
        var firstUser = await dbContext.Users.FirstOrDefaultAsync();
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
//app.MapGroup("/Service").MapServicesApi();

app.Run();