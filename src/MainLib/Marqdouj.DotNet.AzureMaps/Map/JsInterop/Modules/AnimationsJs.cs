using Marqdouj.DotNet.AzureMaps.Map.Animations;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map.JsInterop.Modules
{
    public interface IAzureMapsAnimations
    {
        /// <summary>
        /// Retrieves the name of all the built in easing functions.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetEasingNames();

        /// <summary>
        /// Animates the update of coordinates on a shape.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task AnimateShape(ShapeAnimationOptions options);
    }

    internal class AnimationsJs(AzureMapDotNetRef mapReference) : IAzureMapsAnimations
    {
        private readonly AzureMapDotNetRef mapReference = mapReference;
        private string MapId => mapReference.MapId;
        private IJSRuntime JsRuntime => mapReference.JsRuntime;

        public async Task<List<string>> GetEasingNames()
        {
            return await JsRuntime.InvokeAsync<List<string>>(GetJsInteropMethod(), MapId);
        }

        public async Task AnimateShape(ShapeAnimationOptions options)
        {
            await JsRuntime.InvokeVoidAsync(GetJsInteropMethod(), MapId, options.SerializeToJsInteropJson());
        }

        private static string GetJsInteropMethod([CallerMemberName] string name = "")
            => MapExtensions.GetJsModuleMethod(JsModule.Animations, name);
    }
}
