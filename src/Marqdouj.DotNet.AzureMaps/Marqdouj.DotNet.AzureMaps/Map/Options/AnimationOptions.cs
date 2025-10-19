using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    public enum AnimationType
    {
        Jump,
        Ease,
        Fly
    }

    /// <summary>
    /// The options for animating changes to the map control's camera.
    /// </summary>
    public class AnimationOptions
    {
        /// <summary>
        /// The duration of the animation in milliseconds.
        /// Default '1000'.
        /// </summary>
        public int? Duration { get; set; } = 1000;

        /// <summary>
        /// The type of animation.
        /// "jump" is an immediate change.
        /// "ease" is a gradual change of the camera's settings.
        /// "fly" is a gradual change of the camera's settings following an arc resembling flight.
        /// Default 'jump'.
        /// </summary>
        [JsonIgnore]
        public AnimationType? Type { get; set; }

        [JsonInclude]
        [JsonPropertyName("type")]
        internal string? TypeJs { get => Type.EnumToJsonN(); set => Type = value.JsonToEnumN<AnimationType>(); }

        public override string ToString()
        {
            return $"{Type}:{Duration}";
        }
    }
}
