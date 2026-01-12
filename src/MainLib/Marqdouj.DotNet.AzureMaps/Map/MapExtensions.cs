using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map
{
    internal enum JsModule
    {
        MapFactory,
        Maps,
        Controls,
        Events,
        Features,
        Layers,
        Markers,
        Popups,
        Sources,
    }

    internal static class MapExtensions
    {
        internal const string LIBRARY_NAME = "marqdoujAzureMaps";

        private static string GetNamespace(this JsModule jsModule) => jsModule.ToString();

        internal static string GetJsModuleMethod(JsModule module, [CallerMemberName] string name = "")
            => $"{LIBRARY_NAME}.{module.GetNamespace()}.{name.ToJsonName()}";

        /// <summary>
        /// Generates a valid Css Id using a Guid.
        /// </summary>
        /// <returns></returns>
        internal static string GetRandomCssId()
        {
            return $"g_{Guid.NewGuid()}";
        }

        /// <summary>
        /// first char must be lowercase
        /// </summary>
        internal static string ToJsonName(this string name)
        {
            var firstChar = name[0].ToString().ToLower();
            var remainder = name.Substring(1);
            return $"{firstChar}{remainder}";
        }

        /// <summary>
        /// first char must be lowercase
        /// </summary>
        internal static string ToJsonName<T>(this T value) where T : Enum
        {
            return ToJsonName(value.ToString());
        }

        internal static void EnsureCount(this List<double> items, int min, int? max = null, double addDefault = 0)
        {
            while (items.Count < min)
            {
                items.Add(addDefault);
            }

            //Remove excess values
            if (max != null)
            {
                while (items.Count > max)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        internal static void EnsureCount(this List<Position> items, int min, int? max = null)
        {
            while (items.Count < min)
            {
                items.Add(new Position(0, 0));
            }

            //Remove excess values
            if (max != null)
            {
                while (items.Count > max)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        internal static string EnumToJson<T>(this T value, string underscoreReplacement = "-") where T : Enum
        {
            return value.ToString().ToLower().Replace("_", underscoreReplacement);
        }

        internal static string? EnumToJsonN<T>(this T? value, string underscoreReplacement = "-") where T : struct, Enum
        {
            return value?.ToString().ToLower().Replace("_", underscoreReplacement);
        }

        internal static List<string> EnumToJson<T>(this IEnumerable<T>? items, string underscoreReplacement = "-") where T : Enum
            => items?.Select(e => e.EnumToJson(underscoreReplacement)).Distinct().OrderBy(e => e).ToList() ?? [];

        internal static T JsonToEnum<T>(this string? value, T defaultValue = default!) where T : Enum
        {
            value = value?.Replace("-", "_");
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        internal static T? JsonToEnumN<T>(this string? value, T? defaultValue = (T?)null) where T : struct, Enum
        {
            value = value?.Replace("-", "_");
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        internal static List<T> JsonToEnum<T>(this IEnumerable<string>? items) where T : Enum
        {
            var toProcess = items?.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();
            var values = toProcess?.Select(e => e.JsonToEnum<T>()).ToList() ?? [];
            return [.. values.Select(e => e)];
        }

        internal static void EnsureHasId(this JSInteropDef item)
        {
            if (string.IsNullOrWhiteSpace(item.Id))
                item.Id = MapExtensions.GetRandomCssId();
        }

        internal static void EnsureHasId<T>(this IEnumerable<T> items) where T : JSInteropDef
        {
            if (items is null)
                return;

            foreach (var item in items)
                item.EnsureHasId();
        }
    }
}
