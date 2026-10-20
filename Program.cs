using HospitalAppointmentSystem.Extensions;

// Creates the application builder.
var builder = WebApplication.CreateBuilder(args);

// Registers MVC controllers and views.
builder.Services.AddControllersWithViews();

// Enables session state management.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Sets the session timeout to 20 minutes.
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

// Registers in-memory caching services.
builder.Services.AddMemoryCache();

// Enables response caching services.
builder.Services.AddResponseCaching();

// Builds the web application.
var app = builder.Build();

// Enables error handling outside development.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Redirects HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enables serving CSS, JavaScript and other static files.
app.UseStaticFiles();

// Enables routing.
app.UseRouting();

// Enables response caching middleware.
app.UseResponseCaching();

// Enables session middleware.
app.UseSession();

// Enables authorization middleware.
app.UseAuthorization();

// Configures the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctor}/{action=Index}/{id?}");

// Starts the application.
app.Run();