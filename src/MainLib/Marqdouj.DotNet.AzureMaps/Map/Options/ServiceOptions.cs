namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Global properties used in all atlas service requests.
    /// </summary>
    public class ServiceOptions : ICloneable
    {
        /// <summary>
        /// Disable telemetry collection
        /// This option may only be set when initializing the map.
        /// Default 'true'.
        /// </summary>
        public bool? DisableTelemetry { get; set; }

        /// <summary>
        /// Enable the accessibility feature to provide screen reader support for users who have difficulty visualizing the web application.
        /// This property is set to true by default.
        /// Default 'true'.
        /// </summary>
        public bool? EnableAccessibility { get; set; }

        /// <summary>
        /// Enable the fallback to the REST API geocoder for detecting location accessibility if extracting location from vector data fails.
        /// Disabling this option will prevent the generation of geocode API requests but may lead to a lack of location information for screen readers.
        /// Default 'true'.
        /// </summary>
        public bool? EnableAccessibilityLocationFallback { get; set; }

        /// <summary>
        /// Controls the duration of the fade-in/fade-out animation for label collisions, in milliseconds.
        /// This setting affects all symbol layers.
        /// This setting does not affect the duration of runtime styling transitions or raster tile cross-fading.
        /// Default '300'.
        /// </summary>
        public int? FadeDuration { get; set; }

        /// <summary>
        /// Defines a CSS font-family for locally overriding generation of glyphs in the
        /// 'CJK Unified Ideographs', 'Hiragana', 'Katakana' and 'Hangul Syllables' ranges.
        /// In these ranges, font settings from the map's style will be ignored,
        /// except for font-weight keywords (light/regular/medium/bold). Set to false,
        /// to enable font settings from the map's style for these glyph ranges.
        /// The purpose of this option is to avoid bandwidth-intensive glyph server requests.
        /// Default 'sans-serif'.
        /// </summary>
        public string? LocalIdeographFontFamily { get; set; }

        /// <summary>
        /// Maximum number of images (raster tiles, sprites, icons) to load in parallel,
        /// which affects performance in raster-heavy maps. 16 by default.
        /// </summary>
        public int? MaxParallelImageRequests { get; set; }

        /// <summary>
        /// A boolean that specifies if vector and raster tiles should be reloaded when they expire (based on expires header).
        /// This is useful for data sets that update frequently. When set to false, each tile will be loaded once, 
        /// when needed, and not reloaded when they expire.
        /// Default true
        /// </summary>
        public bool? RefreshExpiredTiles { get; set; }

        /// <summary>
        /// The style API version used when requesting styles and stylesets
        /// </summary>
        public string? StyleAPIVersion { get; set; }

        /// <summary>
        /// The style definitions version to request when requesting styles
        /// from styleDefinitionsPath.
        /// </summary>
        public string? StyleDefinitionsVersion { get; set; }

        /// <summary>
        /// Number of web workers instantiated on a page.
        /// By default, it is set to half the number of CPU cores (capped at 6).
        /// </summary>
        public int? WorkerCount { get; set; }

        /// <summary>
        /// True to validate styles before it's getting applied.
        /// Validation takes significant(few hundred ms) time to process styles during initial load.
        /// Default 'false'.
        /// </summary>
        public bool? ValidateStyle { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
