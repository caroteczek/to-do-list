using ToDoList.Domain;
using ToDoList.Infrastructure.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


string? connection_string = builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("ToDoListDb");
if (connection_string is null)
{
    throw new InvalidOperationException("Connection string 'ToDoListDb' not found");
}
builder.Services.AddScoped<IToDoListRepository, MsSqlToDoListRepository>(x =>
{
    MsSqlToDoListRepository repository = new MsSqlToDoListRepository(connection_string);
    return repository;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Frontend");

app.MapControllers();


app.Run();
