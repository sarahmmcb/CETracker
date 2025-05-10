using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CETrackerApi.Logic;
using CETrackerApi.Api;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")  // Replace with your frontend URL
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });


});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["JwtConfig:Secret"];
        var issuer = builder.Configuration["JwtConfig:ValidIssuer"];
        var audience = builder.Configuration["JwtConfig:ValidAudiences"];

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            IssuerSigningKey = signingKey
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddScoped<IDataConnectionFactory, DataConnectionFactory>();
builder.Services.AddScoped<ICeDataProvider, CeDataProvider>();
builder.Services.AddTransient<IExperienceService, ExperienceService>();
builder.Services.AddTransient<IUnitService, UnitService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILocationService, LocationService>();

var app = builder.Build();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
//}

//app.UseRouting();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

//app.UseAuthorization();

// Exception Handling Middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Log the exception if needed
        Console.WriteLine($"Exception caught: {ex.Message}");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            message = "An unexpected error occurred"
        });
    }
});

app.ConfigureExperiences();
app.ConfigureUnits();
app.ConfigureCategories();
app.ConfigureLocations();


app.Run();
