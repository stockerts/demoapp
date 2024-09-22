var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Prevents client-side access to the cookie
    options.Cookie.IsEssential = true; // Ensures the session cookie is kept even if the user does not consent to non-essential cookies
});

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Home/NotFound");

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
