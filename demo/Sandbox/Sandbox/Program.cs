using Marqdouj.DotNet.Web.Components.Services;
using Microsoft.FluentUI.AspNetCore.Components;
using Sandbox;
using Sandbox.AzureMaps;
using Sandbox.Components;
using Sandbox.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

//Marqdouj.DotNet.AzureMaps
builder.Services.AddMapConfiguration(builder.Configuration);

//Custom interop to Marqdouj.DotNet.AzureMaps
builder.Services.AddScoped<SandboxMaps>();

//Marqdouj.DotNet.Web.Components
builder.Services.AddScoped<IGeolocationService, GeolocationService>();

//Resolves XML Comments for Marqdouj.DotNet.AzureMaps
builder.Services.AddScoped<IAzureMapsXmlService, AzureMapsXmlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
