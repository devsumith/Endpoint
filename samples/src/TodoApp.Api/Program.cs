using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoAppDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TodoApp.Api",
        Description = "",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "",
            Email = ""
        },
        License = new OpenApiLicense
        {
            Name = "Use under MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
        }
    });

    options.EnableAnnotations();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

}).AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.SerializeAsV2 = true);
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp.Api");
        options.RoutePrefix = string.Empty;
        options.DisplayOperationId();
    });

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TodoAppDbContext>();
        db.Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

app.MapPost("/todos", async (Todo todo, TodoAppDbContext context) =>
    {
        context.Todos.Add(todo);
        await context.SaveChangesAsync();

        return Results.Created($"/todos/todos/{todo.Id}",todo);
    })
    .WithName("CreateTodo")
    .Produces<Todo>(StatusCodes.Status201Created);

app.MapGet("/todos", async (TodoAppDbContext context) =>
    await context.Todos.ToListAsync())
    .WithName("GetAllTodos");

app.MapGet("/todos/{id}", async (int id, TodoAppDbContext context) =>
    await context.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound())
    .WithName("GetTodoById")
    .Produces<Todo>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapPut("/todos/{id}", async (int id, Todo inputTodo, TodoAppDbContext context) =>
    {
        var todo = await context.Todos.FindAsync(id);

        if (todo is null) return Results.NotFound();

        todo.Title = inputTodo.Title;
        todo.IsComplete = inputTodo.IsComplete;

        await context.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithName("UpdateTodo")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.MapDelete("/todos/{id}", async (int id, TodoAppDbContext context) =>
    {
        if (await context.Todos.FindAsync(id) is Todo todo)
        {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync();
            return Results.Ok(todo);
        }

        return Results.NotFound();
    })
    .WithName("DeleteTodo")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
}

class TodoAppDbContext : DbContext
{
    public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
