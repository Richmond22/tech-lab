using output_cache.configuration;
using output_cache.configurations;
using output_cache.endpoints;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddDistributedRedisCache(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseOutputCache();

app.MapArticleEndpoints();

app.Run();
