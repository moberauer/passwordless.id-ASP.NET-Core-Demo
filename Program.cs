using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthentication(defaultScheme: OpenIdConnectDefaults.AuthenticationScheme)
    .AddOpenIdConnect(openIdConnectOptions => 
    {
        openIdConnectOptions.SignInScheme = IdentityConstants.ExternalScheme;
        openIdConnectOptions.ResponseType = OpenIdConnectResponseType.Code;
        openIdConnectOptions.ClientId = "https://localhost";
        openIdConnectOptions.MetadataAddress = "https://api.passwordless.id/.well-known/openid-configuration";
    })
    .AddExternalCookie();
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Go to /private to authenticate");
app.MapGet("/private", context => {
    string username = context.User.FindFirst("preferred_username")?.Value ?? string.Empty;
    return context.Response.WriteAsync($"Hello, {username}!");
}).RequireAuthorization();

app.Run();
