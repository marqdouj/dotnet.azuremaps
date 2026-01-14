namespace Marqdouj.DotNet.AzureMaps.Map.Geolocation
{
    /// <summary>
    /// The reason for a Geolocation error, based on <see href="https://developer.mozilla.org/en-US/docs/Web/API/GeolocationPositionError"/>.
    /// </summary>
    public class GeolocationPositionError
    {
        ///// <summary>
        ///// The <see cref="GeolocationPositionErrorCode"/> for the error.
        ///// </summary>
        //public GeolocationPositionErrorCode Code { get; set; }
        public int? Code { get; set; }

        /// <summary>
        /// Details of the error.
        /// </summary>
        public string? Message { get; set; }

        public override string ToString()
        {
            return Message ?? "";
        }
    }
}
