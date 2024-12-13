using BlazeUTS.Service;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);
// Tambahkan layanan Razor Pages
builder.Services.AddRazorPages();

// Konfigurasi lainnya
builder.Services.AddServerSideBlazor();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Add Http CLient
builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://actbackendseervices.azurewebsites.net/");
});

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

//Add Service
builder.Services.AddScoped<CategoryService, CategoryService>();
builder.Services.AddScoped<CourseService, CourseService>();
builder.Services.AddScoped<EnrollmentService, EnrollmentService>();
builder.Services.AddScoped<InstructorService, InstructorService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
