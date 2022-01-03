using Microsoft.EntityFrameworkCore;
using WebTest.Data;
using WebTest.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext, DevApplicationContext>();
}
else
{
    builder.Services.AddDbContext<ApplicationContext, ProdApplicationContext>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.Migrate();
}

app.Run();