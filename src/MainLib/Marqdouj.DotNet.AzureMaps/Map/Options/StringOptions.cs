namespace Marqdouj.DotNet.AzureMaps.Map.Options
{
    public class StringOptions : List<string>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
