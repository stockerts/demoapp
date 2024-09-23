var builder = WebApplication.CreateBuilder(args); // Create new application builder

builder.Services.AddControllersWithViews(); // Adds support for MVC controllers and views

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Session timeout length
    options.Cookie.HttpOnly = true; // Prevents client-side access to the cookie
    options.Cookie.IsEssential = true; // Ensures the session cookie is kept even if the user does not consent to non-essential cookies
    options.Cookie.Name = "demoapp_session"; // Changes the session cookie name from .AspNetCore.Session
});

var app = builder.Build();  // Build the application

app.UseStatusCodePagesWithReExecute("/Home/NotFound"); // Error redirect

app.UseStaticFiles(); // Enable static file presentation like images
app.UseRouting(); // Enable middleware routing
app.UseSession(); // Enable session state

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}"
);  // Create default route without attribute routing enabled

app.Run(); // Run the application
