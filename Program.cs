using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficePortal.Services;
using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficePortal.Data;

var builder = WebApplication.CreateBuilder(args);

#region Localization
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"), // English (United States)
        new CultureInfo("ar-SA")  // Arabic (Saudi Arabia)
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register EmailService with SMTP configuration
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

// Add a middleware to check for mobile devices
builder.Services.AddSingleton<UserAgentService>(); // Register custom service for user agent detection

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();

app.UseAuthorization();

// Middleware to check if the user is on a mobile device and handle redirects
app.Use(async (context, next) =>
{
    var userAgentService = context.RequestServices.GetRequiredService<UserAgentService>();

    // Check if request is from a mobile device and not already on Login page
    if (userAgentService.IsMobileDevice(context.Request) &&
        !context.Request.Path.Equals("/Home/Login", StringComparison.OrdinalIgnoreCase))
    {
        // Redirect mobile users to /Home/Login
        context.Response.Redirect("/Home/Login");
        return;
    }

    // Check if request is from a desktop device and is the initial request (not from Home/Index or other paths)
    if (userAgentService.IsDesktopDevice(context.Request) &&
        !context.Request.Path.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase) &&
        string.IsNullOrEmpty(context.Request.Headers["Referer"])) // No previous page
    {
        // Redirect desktop users to /Home/Index only if this is their first page load
        context.Response.Redirect("/Home/Index");
        return;
    }

    // If no redirection is needed, continue processing the request
    await next();
});

// Set up default routing with fallback
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
