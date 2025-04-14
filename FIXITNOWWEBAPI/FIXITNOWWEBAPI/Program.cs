using FIXITNOWWEBAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Database Context (check connection string)
builder.Services.AddDbContext<FixItNowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Add Controllers with cycle handling!
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });


// ✅ Swagger Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FixItNow API",
        Version = "v1"
    });
});

// ✅ Optional CORS Setup
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost", "http://192.168.24.150")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ✅ Kestrel URL binding
builder.WebHost.UseUrls("http://0.0.0.0:5074");

var app = builder.Build();

// ✅ Developer error page & Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ✅ Detailed errors!

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FixItNow API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
