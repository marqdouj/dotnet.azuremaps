using Marqdouj.DotNet.AzureMaps.Map.Images;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    public interface IAzureMapsImageSprite
    {
        Task<bool> Add(string id, string icon, StyleImageMetadata? meta);
        Task<bool> Add(string id, ImageData icon, StyleImageMetadata? meta);
        Task Clear();
        Task<bool> CreateFromTemplate(ImageTemplateDef templateDef);
        Task<List<string>> GetImageIds();
        Task<bool> HasImage(ImageTemplateDef templateDef);
        Task<bool> HasImage(string id);
        Task Remove(string id);
    }

    internal class ImageSpriteJs(AzureMapDotNetRef mapReference) : IAzureMapsImageSprite
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task<bool> CreateFromTemplate(ImageTemplateDef templateDef)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId, templateDef);
        }

        public async Task<bool> HasImage(ImageTemplateDef templateDef)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId, templateDef.Id);
        }

        public async Task<bool> HasImage(string id)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId, id);
        }

        public async Task<bool> Add(string id, string icon, StyleImageMetadata? meta)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId, id, icon, meta);
        }

        public async Task<bool> Add(string id, ImageData icon, StyleImageMetadata? meta)
        {
            return await JsRuntime.InvokeAsync<bool>(GetJsInteropMethod(), MapId, id, icon, meta);
        }

        public async Task Clear()
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId);
        }

        public async Task<List<string>> GetImageIds()
        {
            return await JsRuntime.InvokeAsync<List<string>>(GetJsInteropMethod(), MapId);
        }

        public async Task Remove(string id)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, id);
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.ImageSprite, name);
    }
}
