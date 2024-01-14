using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Todos");
builder.Services.AddDbContext<TodosContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
var app = builder.Build();


app.MapGet("/todos", async ([FromServices] TodosContext Todos) =>
{
    var allTodos = await Todos.Items.ToListAsync();
    return allTodos;
});

app.MapPost("/todos", async ([FromServices] TodosContext Todos, [FromBody] Item newItem) =>
{
    await Todos.Items.AddAsync(newItem);
    await Todos.SaveChangesAsync();
    return newItem;
});

app.MapPut("/todos/{id}", async ([FromServices] TodosContext Todos, [FromBody] Item updatedItem, [FromRoute] int id) =>
{
    Todos.Items.Where(item => item.Id == id).ExecuteUpdate(b => b.SetProperty(u => u.IsComplete, updatedItem.IsComplete));
    await Todos.SaveChangesAsync();
    return updatedItem;
});

app.MapDelete("/todos/{id}", async ([FromServices] TodosContext Todos, [FromRoute] int id) =>
{
    await Todos.Items.Where(item => item.Id == id).ExecuteDeleteAsync();
    await Todos.SaveChangesAsync();
    return "Item removed";
});

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.Run();
