using CustodyManagementApi.Data;
using CustodyManagementApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CustodyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapGet("/", () => Results.Ok(new
{
    service = "CustodyManagementApi",
    status = "Running",
    docs = "/swagger"
}));
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustodyDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();
