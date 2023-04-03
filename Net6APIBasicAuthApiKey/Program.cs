//https://fast-endpoints.com
//https://deviq.com/design-patterns/repr-design-pattern

using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpLogging;
using Net6APIBasicAuthApiKey.Auth;
using Net6APIBasicAuthApiKey.Helpers;
using Net6APIBasicAuthApiKey.Models;
using NLog.Web;
using NSwag;

using static Net6APILogic.LogicDIRegistrationExtension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ServiceAccessInfo>(builder.Configuration.GetSection(nameof(ServiceAccessInfo)));
builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).UseNLog();

if (bool.TryParse(builder.Configuration.GetSection("HttpLoggingIsEnabled").Value, out bool httpLoging) && httpLoging)
{
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = int.MaxValue;
        logging.ResponseBodyLogLimit = int.MaxValue;
        logging.MediaTypeOptions.AddText("application/json");

    });
}
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
});
if (int.TryParse(builder.Configuration.GetSection("HTTPS_PORT").Value, out int httpsPort))
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
        options.HttpsPort = httpsPort;
    });
}
var serviceAccessInfo = new ServiceAccessInfo();
builder.Configuration.GetSection(nameof(ServiceAccessInfo)).Bind(serviceAccessInfo);
builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultScheme = AuthenticationHandler.BasicAuthenticationSchemeName;
    })
    .AddScheme<BasicAuthenticationOptions, AuthenticationHandler>(AuthenticationHandler.BasicAuthenticationSchemeName, o => o.ServiceAccessInfo = serviceAccessInfo);
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddAuthorization();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new NullableDateOnlyConverter());
});
builder.Services.AddSwaggerDoc(s => 
{
    s.AddAuth("ApiKey", new()
    {
        Name = AuthorizationService.ApiKeyHeaderValue,
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.ApiKey,
    });
    s.AddAuth("Basic", new()
    {
        Name = "Basic",
        In = OpenApiSecurityApiKeyLocation.Header,
        Type = OpenApiSecuritySchemeType.Basic,
    });
});


builder.Services.APILogicRegistration();

using var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
    app.UseSwaggerUI();
}
app.Run();
