using Clinic.Application.Dependencies;
using Clinic.Infrastructure.DependencyInjection;
using Clinic.web.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add<ClinicSettingsFilter>();
    })
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddApplicationDependencies();
builder.Services.AuthenticationServices(builder.Configuration);
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddTransient<ClinicSettingsFilter>();
builder.Services.AddHttpContextAccessor();

// Override the default auth scheme to Identity cookie for the MVC layer
builder.Services.Configure<AuthenticationOptions>(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme    = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme       = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme      = IdentityConstants.ApplicationScheme;
    options.DefaultForbidScheme       = IdentityConstants.ApplicationScheme;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath         = "/Account/Login";
    options.LogoutPath        = "/Account/Logout";
    options.AccessDeniedPath  = "/Account/Login";
    options.ExpireTimeSpan    = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// Localization — English (default) + Arabic
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("ar-EG")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures     = supportedCultures;
    options.SupportedUICultures   = supportedCultures;
    // Allow cookie-based culture persistence
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// MUST come before UseAuthentication so culture is set early
app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
