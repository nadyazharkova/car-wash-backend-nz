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

app.UseAuthentication();
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

static bool CheckLoginAndPassword(string login, string password)
{
    return true;
}

app.MapPost("/login", async ([FromQuery] string login, [FromQuery] string password) =>
{
    // Проверка логина и пароля
    // Например, здесь должна быть логика проверки в базе данных
    bool isValidLoginAndPassword = CheckLoginAndPassword(login, password);


    if (!isValidLoginAndPassword)
    {
        return Results.Unauthorized();
    }
    var roleId = "";
    var userId = "";

    // Генерация токена
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // ID пользователя
            new Claim("Role", roleId.ToString()), // ID роли
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])), SecurityAlgorithms.HmacSha256Signature)
    };
    
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(securityToken);

    return Results.Ok(new { token });
}).WithDisplayName("Login");

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/") && context.Request.Method == HttpMethod.Get.Method)
    {
        var authHeader = context.Request.Headers["Authorization"];
        //if (authHeader != "" && authHeader.ToString().StartsWith("Bearer "))
        if (authHeader.Count > 0)
        {
            var token = authHeader[1];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            using (var dbContext = new CarWashContext())
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == idClaim);
                if (user != null)
                {
                    context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.UserData, user.RoleId.ToString()),
                        new Claim(ClaimTypes.Actor, user.PersonId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                        // new Claim(ClaimTypes.SerialNumber, firstUser.Login),
                        // new Claim(ClaimTypes.UserData, firstUser.Password),
                        // new Claim(ClaimTypes.NameIdentifier, firstUser.RoleId.ToString()),
                    }));
                }
            }
            if (roleClaim == "owner")
            {
                // Действия для владельцев
            }
            else if (roleClaim == "client")
            {
                // Действия для клиентов
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }
    }
    await next.Invoke();
});


app.Run();