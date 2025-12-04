using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// The options for enabling/disabling user interaction with the map.
    /// </summary>
    public class UserInteractionOptions : ICloneable
    {
        /// <summary>
        /// Whether the Shift + left click and drag will draw a zoom box.
        /// Default 'true'.
        /// </summary>
        public bool? BoxZoomInteraction { get; set; }

        /// <summary>
        /// Whether double left click will zoom the map inwards.
        /// Default 'true'.
        /// </summary>
        public bool? DblClickZoomInteraction { get; set; }

        /// <summary>
        /// Whether left click and drag will pan the map.
        /// Default 'true'.
        /// </summary>
        public bool? DragPanInteraction { get; set; }

        /// <summary>
        /// Whether right click and drag will rotate and pitch the map.
        /// Default 'true'.
        /// </summary>
        public bool? DragRotateInteraction { get; set; }

        /// <summary>
        /// Whether the map is interactive or static. If false, all user interaction is disabled.  
        /// If true, only selected user interactions will enabled.
        /// Default 'true'.
        /// </summary>
        public bool? Interactive { get; set; }

        /// <summary>
        /// Whether the keyboard interactions are enabled.
        /// Default 'true'.
        /// </summary>
        public bool? KeyboardInteraction { get; set; }

        /// <summary>
        /// Whether the map should zoom on scroll input.
        /// Default 'true'.
        /// </summary>
        public bool? ScrollZoomInteraction { get; set; }

        /// <summary>
        /// Whether touch interactions are enabled for touch devices.
        /// Default 'true'.
        /// </summary>
        public bool? TouchInteraction { get; set; }

        /// <summary>
        /// Whether touch rotation is enabled for touch devices. This option is not applied if touchInteraction is disabled.
        /// Default 'true'.
        /// </summary>
        public bool? TouchRotate { get; set; }

        /// <summary>
        /// Sets the zoom rate of the mouse wheel
        /// Default '1/450'.
        /// </summary>
        public double? WheelZoomRate { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
