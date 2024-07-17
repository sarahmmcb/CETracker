using CETrackerApi.Logic;
using CETrackerApi.Api;
using Microsoft.OpenApi.Models;
using CETrackerDAL.DataAccess;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.SetIsOriginAllowed(origin => true).AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddScoped<IDataConnectionFactory, DataConnectionFactory>();
builder.Services.AddTransient<IDataAccess, DataAccess>();
builder.Services.AddTransient<IExperienceService, ExperienceService>();
builder.Services.AddTransient<ICeDataProvider, CeDataProvider>();
builder.Services.AddTransient<IUnitService, UnitService>();
builder.Services.AddTransient<IUnitData, UnitData>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryData, CategoryData>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ILocationData, LocationData>();

var app = builder.Build();

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

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.ConfigureExperiences();
app.ConfigureUnits();
app.ConfigureCategories();
app.ConfigureLocations();

app.Run();
