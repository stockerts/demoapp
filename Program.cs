using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args); // Create new application builder

builder.Services.AddControllers();  // Add services to the container.
builder.Services.AddControllersWithViews(); // Adds support for MVC controllers and views
builder.Services.AddHttpClient(); // Adds support for server-side requests

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Session timeout length
    options.Cookie.HttpOnly = true; // Prevents client-side access to the cookie
    options.Cookie.IsEssential = true; // Ensures the session cookie is kept even if the user does not consent to non-essential cookies
    options.Cookie.Name = "demoapp_session"; // Changes the session cookie name from .AspNetCore.Session
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Demo Bank API",
        Description = "Use to create mock requests and responses. Not an official F5 property."
    });

    // Include XML comments for better documentation (keep this as is)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Add example values for MessageModel properties
    options.MapType<demobankapi.Controllers.messageservice.MessageModel>(() => new OpenApiSchema
    {
        Type = "object",
        Properties =
        {
            ["firstName"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("Anthony")
            },
            ["lastName"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("Stark")
            },
            ["phoneNumber"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("(123) 456-7890")
            },
            ["email"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("tony.stark@starkindustries.com")
            },
            ["message"] = new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("Hello, this is a sample message.")
            }
        }
    });
});

var app = builder.Build();  // Build the application

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Bank API V1");
    c.DefaultModelsExpandDepth(-1);  // Optional UI tweaks
});

app.UseStatusCodePagesWithReExecute("/Home/NotFound"); // Error redirect

app.UseStaticFiles(); // Enable static file presentation like images
app.UseRouting(); // Enable middleware routing
app.UseSession(); // Enable session state
app.MapControllers();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}"
);  // Create default route without attribute routing enabled

app.Run(); // Run the application
