namespace Marqdouj.DotNet.AzureMaps.Map.Common
{
    public static class Gps
    {
        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="degrees">The angle, in degrees, to convert to radians.</param>
        /// <returns>The angle in radians equivalent to the specified number of degrees.</returns>
        public static double ToRadians(this double degrees) => degrees * Math.PI / 180.0;

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="radians">The angle, in radians, to convert to degrees.</param>
        /// <returns>The equivalent angle measured in degrees.</returns>
        public static double ToDegrees(this double radians) => radians * 180.0 / Math.PI;

        /// <summary>
        /// Calculates the initial bearing, in degrees, from a starting point to a destination point specified by their
        /// latitude and longitude coordinates.
        /// </summary>
        /// <remarks>The bearing is calculated using the great-circle path between the two points on the
        /// Earth's surface. This method assumes a spherical Earth and does not account for ellipsoidal
        /// effects.</remarks>
        /// <param name="lat1">The latitude of the starting point, in decimal degrees. Positive values indicate north latitude; negative
        /// values indicate south latitude.</param>
        /// <param name="lon1">The longitude of the starting point, in decimal degrees. Positive values indicate east longitude; negative
        /// values indicate west longitude.</param>
        /// <param name="lat2">The latitude of the destination point, in decimal degrees. Positive values indicate north latitude; negative
        /// values indicate south latitude.</param>
        /// <param name="lon2">The longitude of the destination point, in decimal degrees. Positive values indicate east longitude;
        /// negative values indicate west longitude.</param>
        /// <returns>The initial bearing from the starting point to the destination point, in degrees, measured clockwise from
        /// true north. The value is in the range 0 to less than 360.</returns>
        public static double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
        {
            // Convert coordinates to radians
            double φ1 = lat1.ToRadians();
            double φ2 = lat2.ToRadians();
            double Δλ = (lon2 - lon1).ToRadians();

            // Bearing formula
            double y = Math.Sin(Δλ) * Math.Cos(φ2);
            double x = Math.Cos(φ1) * Math.Sin(φ2) -
                       Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(Δλ);

            double θ = Math.Atan2(y, x); // radians

            // Convert to degrees and normalize to 0-360
            double bearing = (θ.ToDegrees() + 360) % 360;
            return bearing;
        }
    }
}
