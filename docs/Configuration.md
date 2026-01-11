## DotNet.AzureMaps Documentation - Configuration

### [<- Go Back](ReadMe.md)

### [App.Razor](../src/MainLib/Sandbox/Components/App.razor)
- Add the Azure Maps SDK scripts to the `head`.
- Add the `marqdouj-azuremaps.js` to the `body` after the `_framework/blazor.web.js` script.
- If required, add the anonymous authentication script after the `marqdouj-azuremaps.js` script.
- If required, add the SasToken authentication script after the `marqdouj-azuremaps.js` script.

### [MapSetup.cs](../src/MainLib/Sandbox/MapSetup.cs)
`MapSetup.cs` contains examples of all the supported authentication methods.

NOTE: For SasToken authentication, you can provide a SasTokenUrl instead
of configuring the GetSasToken callback in App.Razor.

### [Program.cs](../src/MainLib/Sandbox/Program.cs)
This is where you configure the authentication and global map settings.
```csharp
//Using MapSetup.cs
builder.Services.AddMapConfiguration(builder.Configuration);
```

