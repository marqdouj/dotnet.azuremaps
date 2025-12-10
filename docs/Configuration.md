## DotNet.AzureMaps Documentation - Configuration

### [<- Go Back](ReadMe.md)

### [App.Razor](../src/MainLib/Sandbox/Components/App.razor)
- Add the Azure Maps SDK scripts to the `head`.
- Add the `marqdouj-azuremaps.js` to the `body` after the `_framework/blazor.web.js` script.
- If required, add the anonymous authentication script after the `marqdouj-azuremaps.js` script.

### [MapSetup.cs](../src/MainLib/Sandbox/MapSetup.cs)
`MapSetup.cs` contains examples of all three supported authentication methods.
Normally, you would choose one method and remove unused code/comments.

### [Program.cs](../src/MainLib/Sandbox/Program.cs)
This is where you configure the authentication and global map settings.
```csharp
//Using MapSetup.cs
builder.Services.AddMapConfiguration(builder.Configuration);
```

If you only need to support 'SubscriptionKey' you don't need `MapSetup.cs`, just configure it directly in `Program.cs`:
```csharp
using Marqdouj.DotNet.AzureMaps;
using Marqdouj.DotNet.AzureMaps.Map.Settings;
using Marqdouj.DotNet.AzureMaps.Map.Configuration;

builder.Services.AddMarqdoujAzureMaps(config =>
{
    config.Authentication.Mode = MapAuthenticationMode.SubscriptionKey;
    config.Authentication.SubscriptionKey = builder.Configuration["AzureMaps:SubscriptionKey"];
    //Optional
    config.Options = new MapOptions { Style = new StyleOptions { Language = "auto" } };
    //You can override the global MapOptions by setting the AzureMap.Options parameter.
});
```

