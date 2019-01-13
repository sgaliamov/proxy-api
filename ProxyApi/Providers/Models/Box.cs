using System.Globalization;

namespace ProxyApi.Providers.Models
{
    public sealed class Box
    {
        public float MaxLatitude { get; private set; }
        public float MaxLongitude { get; private set; }
        public float MinLatitude { get; private set; }
        public float MinLongitude { get; private set; }

        public static Box Create(float? minimumLongitude, float? minimumLatitude, float? maximumLongitude, float? maximumLatitude)
        {
            if (minimumLatitude.HasValue && minimumLongitude.HasValue && maximumLatitude.HasValue && maximumLongitude.HasValue)
            {
                return new Box
                {
                    MaxLatitude = maximumLatitude.Value,
                    MaxLongitude = maximumLongitude.Value,
                    MinLatitude = minimumLatitude.Value,
                    MinLongitude = minimumLongitude.Value
                };
            }

            return null;
        }

        public string ToBbox()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0},{1},{2},{3}",
                MinLongitude,
                MinLatitude,
                MaxLongitude,
                MaxLatitude);
        }
    }
}
