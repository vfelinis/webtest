using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using WebTest.Data;
using WebTest.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardLimit = 1;
    options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext, DevApplicationContext>();
}
else
{
    builder.Services.AddDbContext<ApplicationContext, ProdApplicationContext>();
}

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/", (ApplicationContext dbContext) =>
{
    return dbContext.Users.ToList();
});
app.MapPost("/", (string firstName, string lastName, ApplicationContext dbContext) =>
{
    var user = new User
    {
        FirstName = firstName,
        LastName = lastName,
        DateCreated = DateTimeOffset.Now,
    };
    dbContext.Users.Add(user);
    dbContext.SaveChanges();
    return Results.Ok();
});

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    app.Run($"http://*:{Environment.GetEnvironmentVariable("PORT")}");
}
    