using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Order;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddDefaultPolicy(
    corsPolicyBuilder =>
    {
        var clientOrigins = builder.Configuration.GetValue<string>("ClientOrigins")
                                                 .Split(',')
                                                 .Select(x => x.Trim())
                                                 .ToArray();

        corsPolicyBuilder.WithOrigins(clientOrigins)
                         .AllowAnyHeader();
    })
);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(settings =>
    {
        settings.Title = "Order service";
        settings.Version = "v1";
        settings.SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new RequireObjectPropertiesContractResolver(),
        };
        settings.OperationProcessors.Add(new CamelCaseQueryParamsOperationProcessor());
    },
    tagIndex: 2,
    shortSchemaNames: true);

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors();

app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(s => s.ConfigureDefaults());
}

app.Run();

internal class RequireObjectPropertiesContractResolver : CamelCasePropertyNamesContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.PropertyType != null
            && Nullable.GetUnderlyingType(property.PropertyType) == null
            && (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)))
        {
            property.Required = Required.Always;
        }
        return property;
    }
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);
        if (Nullable.GetUnderlyingType(objectType) == null && (!objectType.IsGenericType || objectType.GetGenericTypeDefinition() != typeof(Nullable<>)))
        {
            contract.ItemRequired = Required.Always;
        }
        return contract;
    }
    protected override JsonProperty CreatePropertyFromConstructorParameter(JsonProperty? matchingMemberProperty, ParameterInfo parameterInfo)
    {
        var property = base.CreatePropertyFromConstructorParameter(matchingMemberProperty, parameterInfo);
        if (property.PropertyType != null
            && Nullable.GetUnderlyingType(property.PropertyType) == null
            && (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)))
        {
            property.Required = Required.Default;
        }
        return property;
    }
}