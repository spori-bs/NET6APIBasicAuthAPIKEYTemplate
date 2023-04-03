.NET 6 WebApi using https://fast-endpoints.com.
When you use this template for classic AspNetCore authentication do not forget to use [proper authorization rules](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/simple).

Key features:
1. Basic Auth, with simple API-KEY validation (Http 401 if basic auth failed. Http 403 if apikey validation failed)
2. Logprovider -> NLog for Microsoft.Extension.Logging with `NLog.config` file.
3. HttpLogging is available. Configurable in appsettings.json.
4. MediatR is available with composite type sample post method.
5. Separated Request/Response objects (DTOs) in Shared lib.
