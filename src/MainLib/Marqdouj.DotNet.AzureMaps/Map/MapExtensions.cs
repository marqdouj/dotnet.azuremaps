using Marqdouj.DotNet.AzureMaps.Map.Controls;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Map
{
    internal static class MapExtensions
    {
        internal const string LIBRARY_NAME = "marqdoujAzureMaps";

        internal static string GetMapInteropMethod(string module, [CallerMemberName] string name = "")
            => $"{MapExtensions.LIBRARY_NAME}.MapInterop.{module}.{name.ToJsonName()}";

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

        internal static List<object>? ToJson(this IEnumerable<MapControl>? controls)
            => controls?.Select(e => (object)e).ToList();

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

        public static string EnumToJson<T>(this T value) where T : Enum
        {
            return value.ToString().ToLower().Replace("_", "-");
        }

        public static string? EnumToJsonN<T>(this T? value) where T : struct, Enum
        {
            return value?.ToString().ToLower().Replace("_", "-");
        }

        public static List<string> EnumToJson<T>(this IEnumerable<T>? items) where T : Enum
            => items?.Select(e => e.EnumToJson()).Distinct().OrderBy(e => e).ToList() ?? [];

        public static T JsonToEnum<T>(this string? value, T defaultValue = default!) where T : Enum
        {
            value = value?.Replace("-", "_");
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        public static T? JsonToEnumN<T>(this string? value, T? defaultValue = (T?)null) where T : struct, Enum
        {
            value = value?.Replace("-", "_");
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        public static List<T> JsonToEnum<T>(this IEnumerable<string>? items) where T : Enum
        {
            var toProcess = items?.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();
            var values = toProcess?.Select(e => e.JsonToEnum<T>()).ToList() ?? [];
            return [.. values.Select(e => e)];
        }
    }
}
