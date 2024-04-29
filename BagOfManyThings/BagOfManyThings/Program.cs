using BagOfManyThings.Client.Pages;
using BagOfManyThings.Components;
using BagOfManyThings.Components.Account;
using BagOfManyThings.Data;
using KristofferStrube.Blazor.FileSystem;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()//makes it so that it uses the server AND web assembly renders
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();//important for components to know authentication state
builder.Services.AddScoped<IdentityUserAccessor>();//This is what helps us reach the user and retrieve info about logged in users
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();//Maintains authentication on client and server side

builder.Services.AddAuthentication(options =>//Configures Atuhentication Options and uses the default when a specific scheme isn't requested
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>//Adds Database Context
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => //IdentityCore gives more extensive control
{
    options.SignIn.RequireConfirmedAccount = false; // Disable 2FA requirement
    options.User.RequireUniqueEmail = false; // Allow duplicate emails
    options.SignIn.RequireConfirmedEmail = false; // Disable email confirmation
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()//We say that we want to use EntityFramework with our ApplicationDbContext which lets us interact with our Database using C# code
    .AddSignInManager();//is necessary for signing in and signing out and adds some nice functions for that

builder.Services.AddStorageManagerService();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>(); //configures email sender

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await roleManager.CreateAsync(new IdentityRole("Admin"));
    await roleManager.CreateAsync(new IdentityRole("Player"));
    await roleManager.CreateAsync(new IdentityRole("DM"));
}

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(BagOfManyThings.Client._Imports).Assembly);//add razor WASM and server side rendering functionality

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();//gives you the option to add authentication endpoints like register, login, 2fa, manage your acount etc

app.Run();
