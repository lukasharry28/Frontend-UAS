using BlazeUTS.Service;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Polly;
using Polly.Extensions.Http;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebApplication.CreateBuilder(args);
// Tambahkan layanan Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => 
    { 
        options.DetailedErrors = true;
    });

// Konfigurasi lainnya
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddAuthorization();

// Tambahkan ini untuk menangani prerendering
// Matikan prerendering dengan cara yang benar
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.RootDirectory = "/Pages";
});

// Nonaktifkan endpoint routing di level MVC
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

// Add services to the container.
// builder.Services.AddRazorComponents()
//     .AddInteractiveServerComponents();

//Add Service
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<CategoryService, CategoryService>();
builder.Services.AddScoped<CourseService, CourseService>();
builder.Services.AddScoped<EnrollmentService, EnrollmentService>();
builder.Services.AddScoped<InstructorService, InstructorService>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<LoginService, LoginService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthMessageHandler>();

// Add authorization state provider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

//Add Http CLient
builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddPolicyHandler(GetRetryPolicy());

builder.Services.AddHttpClient<CourseService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
});

builder.Services.AddHttpClient<EnrollmentService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
});

builder.Services.AddHttpClient<InstructorService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
});


builder.Services.AddHttpClient<LoginService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddPolicyHandler(GetRetryPolicy())
    .AddHttpMessageHandler<AuthMessageHandler>();

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
});

// Configure JWT authentication with more secure parameters
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://actbackendseervices.azurewebsites.net/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"]
        .FirstOrDefault()?.Replace("Bearer ", "");

    if (!string.IsNullOrEmpty(token))
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = "Token expired" });
                return;
            }
        }
        catch
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "Invalid token" });
            return;
        }
    }

    await next.Invoke();
});

// Add retry policy method
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
