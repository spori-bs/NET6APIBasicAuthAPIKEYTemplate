//https://fast-endpoints.com
//https://deviq.com/design-patterns/repr-design-pattern

using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Net6APIBasicAuthApiKey.Auth;
using Net6APIBasicAuthApiKey.Helpers;
using Net6APIBasicAuthApiKey.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ServiceAccessInfo>(builder.Configuration.GetSection(nameof(ServiceAccessInfo)));


builder.Services.AddControllers();
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


using var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.Run();
