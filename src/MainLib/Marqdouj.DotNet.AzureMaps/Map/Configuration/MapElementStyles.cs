namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    /// <summary>
    /// The style of the map element.​
    /// </summary>
    public class MapElementStyles : ICloneable
    {
        /// <summary>
        /// Specifies the visibility of the element.​
        /// </summary>
        public bool? Visible { get; set; }
        override public string ToString()
        {
            return $"{Visible}";
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
