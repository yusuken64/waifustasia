using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waifustasia.Data;
using Waifustasia.Data.Authentication;
using Waifustasia.Data.Automation;
using Waifustasia.Data.Chat;
using Waifustasia.Data.Memory;
using Waifustasia.Data.User;
using Waifustasia.Data.Waifu;
using Waifustasia.OpenAI;
using Waifustasia.WaifuAI;
using Waifustasia.WaifuAI.Imagaes;
using Waifustasia.WaifuAI.Settings;

var builder = WebApplication.CreateBuilder(args);

//Authentication
builder.Services.AddIdentity<Waifustasia.Data.User.User, IdentityRole>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<WaifustasiaDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<WaifustasiaAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WaifustasiaAuthenticationStateProvider>());

//Waifu

IConfigurationRoot configuration;
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == Environments.Production)
{
    configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.Production.json", optional: true)
        .Build();
}
else
{
    configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.Development.json", optional: true)
        .Build();
}

builder.Services.AddDbContext<WaifustasiaDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<WaifustasiaDbContext>();

builder.Services.AddScoped<IWaifuRepository, WaifuRepository>();
builder.Services.AddScoped<IWaifuService, WaifuService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IMemoryRepository, MemoryRepository>();
builder.Services.AddScoped<IMemoryService, MemoryService>();

builder.Services.AddScoped<IIntellegenceService, IntellegenceService>();
//builder.Services.AddScoped<IImageStore, LocalImageStore>();
builder.Services.AddScoped<IImageStore, AzureImageStore>();
builder.Services.AddScoped<AzureSettingStore<AzureRecord>>();

builder.Services.AddScoped<MemoryGenerator>();
builder.Services.AddHostedService<MemoryGeneratorService>();

var app = builder.Build();

PrepareDb.PreparePopulation(app);

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();