namespace Marqdouj.DotNet.AzureMaps.Map.Geolocation
{
    /// <summary>
    /// Geolocation event data. Provides the <see cref="GeolocationResult"/> with the position or error associated with the event.
    /// </summary>
    public class GeolocationEventArgs : EventArgs
    {
        internal GeolocationEventArgs(GeolocationResult result)
        {
            GeolocationResult = result;
        }

        /// <summary>
        /// The <see cref="GeolocationResult"/> associated with the event.
        /// </summary>
        public GeolocationResult GeolocationResult { get; }
    }
}
