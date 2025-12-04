namespace Marqdouj.DotNet.AzureMaps.Map.Configuration
{
    public class StringOptions : List<string>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
