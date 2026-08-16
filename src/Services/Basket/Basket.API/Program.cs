var builder = WebApplication.CreateBuilder(args);

// Add services to the container

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviors<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Configure the HTTP request pipeline
var app = builder.Build();

app.MapCarter();

app.Run();
