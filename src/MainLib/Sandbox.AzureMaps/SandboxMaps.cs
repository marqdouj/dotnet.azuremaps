using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Sandbox.AzureMaps
{
    public class SandboxMaps(IJSRuntime jsRuntime) : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Sandbox.AzureMaps/sandboxMaps.js").AsTask());

        public async ValueTask<bool> MapExists(string mapId)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<bool>(GetCustomMapMethod(), mapId);
        }

        public async ValueTask<string> AddControls(string mapId)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>(GetCustomMapMethod(), mapId);
        }

        public async ValueTask<string> RemoveControls(string mapId)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>(GetCustomMapMethod(), mapId);
        }

        internal static string GetCustomMapMethod([CallerMemberName] string name = "")
        {
            return name.ToJsonName();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (moduleTask.IsValueCreated)
                {
                    var module = await moduleTask.Value;
                    await module.DisposeAsync();
                }
            }
            catch (JSDisconnectedException)
            {
            }
        }
    }

    internal static class Extensions
    {
        /// <summary>
        /// first char must be lowercase
        /// </summary>
        public static string ToJsonName(this string name)
        {
            var firstChar = name[0].ToString().ToLower();
            var remainder = name.Substring(1);
            return $"{firstChar}{remainder}";
        }
    }
}
