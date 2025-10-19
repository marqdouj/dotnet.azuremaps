using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// Override the default styles for the map elements.​
    /// </summary>
    public class StyleOverrides : ICloneable
    {
        /// <summary>
        /// Country or regions.
        /// </summary>
        public BorderedMapElementStyles? CountryRegion { get; set; }

        /// <summary>
        /// First administrative level within the country/region level, such as a state or a province.
        /// </summary>
        public BorderedMapElementStyles? AdminDistrict { get; set; }

        /// <summary>
        /// Second administrative level within the country/region level, such as a county.​
        /// </summary>
        public BorderedMapElementStyles? AdminDistrict2 { get; set; }

        /// <summary>
        /// Building footprints along with their address numbers.​
        /// </summary>
        public MapElementStyles? BuildingFootprint { get; set; }

        /// <summary>
        /// Street blocks in the populated places​.
        /// </summary>
        public MapElementStyles? RoadDetails { get; set; }

        public object Clone()
        {
            var clone = new StyleOverrides
            {
                CountryRegion = (BorderedMapElementStyles?)CountryRegion?.Clone(),
                AdminDistrict = (BorderedMapElementStyles?)AdminDistrict?.Clone(),
                AdminDistrict2 = (BorderedMapElementStyles?)AdminDistrict2?.Clone(),
                BuildingFootprint = (MapElementStyles?)BuildingFootprint?.Clone(),
                RoadDetails = (MapElementStyles?)RoadDetails?.Clone()
            };

            return clone;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}
