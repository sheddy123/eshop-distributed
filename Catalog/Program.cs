var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ProductDbContext>(connectionName: "catalogdb");
builder.Services.AddScoped<ProductService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();
app.UseDeveloperExceptionPage();

if (app.Environment.IsDevelopment())
{
    app.UseMigration();
    app.MapOpenApi();
}

app.MapProductEndpoints();

app.UseHttpsRedirection();

app.Run();
