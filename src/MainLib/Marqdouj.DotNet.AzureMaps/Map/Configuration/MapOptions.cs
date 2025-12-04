namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// Represents configuration options for initializing and customizing a map instance, including camera settings,
    /// style, bounds, and user interaction behaviors.
    /// </summary>
    /// <remarks>Use this class to specify initial map parameters when creating or configuring a map. Each
    /// property provides access to a group of related options, allowing fine-grained control over map appearance and
    /// behavior. All properties are optional; unset properties will use default values defined by the map
    /// implementation.</remarks>
    public class MapOptions
    {
        /// <summary>
        /// The camera configuration to be used when creating the map.
        /// Camera and CameraBounds are mutually exclusive; Camera takes precedence if both are set.
        /// </summary>
        public CameraOptions? Camera { get; set; }

        /// <summary>
        /// The camera bounds configuration to be used when creating the map.
        /// Camera and CameraBounds are mutually exclusive; Camera takes precedence if both are set.
        /// </summary>
        /// <remarks>If set, the camera will be constrained to the specified bounds. If null, the camera
        /// is not restricted and can move freely. This property is typically used to prevent the camera from panning or
        /// zooming outside a designated area.</remarks>
        public CameraBoundsOptions? CameraBounds { get; set; }

        /// <summary>
        /// Gets or sets the configuration options for the service.
        /// </summary>
        public ServiceOptions? Service { get; set; }

        /// <summary>
        /// Gets or sets the style options to be applied to the component.
        /// </summary>
        public StyleOptions? Style { get; set; }

        /// <summary>
        /// Gets or sets the options that control user interaction behavior for this instance.
        /// </summary>
        /// <remarks>Specify user interaction options to customize prompts, confirmations, or other
        /// interactive features. If set to <see langword="null"/>, default interaction behavior will be used.</remarks>
        public UserInteractionOptions? UserInteraction { get; set; }
    }
}
