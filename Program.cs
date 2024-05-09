var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddHttpClient<CatService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/NotFound");

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
