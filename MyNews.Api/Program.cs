using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Threading.RateLimiting;

using MyNews.Api.Background;
using MyNews.Api.Data;
using MyNews.Api.Interfaces;
using MyNews.Api.Middlewares;
using MyNews.Api.Options;
using MyNews.Api.Services;

using OpenAI.Chat;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQL Server DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure options
builder.Services.Configure<BackgroundJobsOptions>(
    builder.Configuration.GetSection("BackgroundJobs"));
builder.Services.Configure<LocalizationOptions>(
    builder.Configuration.GetSection("Localization"));

builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));

builder.Services.AddSingleton<ChatClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
    return new ChatClient(options.Model, options.ApiKey);
});

// Register services (DI)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<ISourceService, SourcesService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<ISectionsService, SectionsService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddScoped<IChatGptService, ChatGptService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();

builder.Services.AddHttpClient<IRssService, RssService>();

if (builder.Environment.IsProduction())
{
    builder.Services.AddHostedService<RssBackgroundService>();
    builder.Services.AddHostedService<CleanupBackgroundService>();
}

var frontendUrl = builder.Configuration["FrontendUrl"];
var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(origins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        // block tokens from unexpected schemes (WebSockets)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

// Configure Swagger for development
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        // Basic Auth
        var authHeader = context.Request.Headers["Authorization"].ToString();
        var allowedUser = builder.Configuration["SwaggerAuth:Username"];
        var allowedPassword = builder.Configuration["SwaggerAuth:Password"];
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{allowedUser}:{allowedPassword}"));

        if (authHeader != $"Basic {encoded}")
        {
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger UI\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }

    await next();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortGlobeNews API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception!");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
    }
});

app.UseRouting();

app.UseCors("AllowAngular");

// Global error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();
app.Run();
