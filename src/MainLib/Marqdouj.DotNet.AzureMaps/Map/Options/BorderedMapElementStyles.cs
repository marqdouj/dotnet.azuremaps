namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    /// <summary>
    /// The style of the bordered map element.
    /// </summary>
    public class BorderedMapElementStyles : ICloneable
    {
        /// <summary>
        /// Specifies the visibility of the border.​
        /// </summary>
        public bool? BorderVisible { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        override public string ToString()
        {
            return $"{BorderVisible}";
        }
    }

}
