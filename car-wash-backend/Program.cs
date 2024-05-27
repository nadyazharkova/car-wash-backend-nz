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

// Настройка Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 1; 
        //options.Password.RequireNonNumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<CarWashContext>();

// Настройка JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173<") // Замените на домен вашего клиента
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

var dbContext = new CarWashContext();
var user = new User();

app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"];
    if (authHeader.Count > 0)
    {
        var token = authHeader[0].Split(' ')[1]; // Извлекаем токен из заголовка Authorization
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Здесь вы можете добавить логику для получения данных пользователя из базы данных
        // на основе информации в токене, например, ID пользователя (sub claim)

        // Пример получения данных пользователя из базы данных
        using (dbContext = new CarWashContext())
        {
            user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            if (user!= null)
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.UserData, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                }));
            }
        }
    }
    await next();
});


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

async Task<bool> CheckLoginAndPassword(string login, string password)
{
    try
    {
        using (dbContext = new CarWashContext())
        {
            user = await dbContext.Users.FirstOrDefaultAsync(u => u.Password == password && u.Login.Contains(login));
            if (user.UserId != null)
                user.Role = dbContext.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);
            return user?.UserId!= null;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error checking login and password: {ex.Message}");
        return false;
    }
}


app.MapPost("/login", async ([FromQuery] string login, [FromQuery] string password) =>
{
    // Проверка логина и пароля
    bool isValidLoginAndPassword = await CheckLoginAndPassword(login, password);
    
    if (!isValidLoginAndPassword)
    {
        return Results.Unauthorized();
    }
    // Генерация токена
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: user.Role.RoleName,
        audience: "your_audience",
        claims: new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // ID пользователя
            new Claim(ClaimTypes.UserData, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString()),
        },
        expires: DateTime.Now.AddDays(1), // Токен истекает через 1 день
        signingCredentials: credentials);

    return Results.Ok(token);
}).WithDisplayName("Login");

app.Run();