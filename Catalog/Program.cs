using FastEndpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog service",
        Version = "v1"
    });

    options.SwaggerGeneratorOptions.TagsSelector = api => new List<string> { api.RelativePath!.Split('/')[1] };

});

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.Run();