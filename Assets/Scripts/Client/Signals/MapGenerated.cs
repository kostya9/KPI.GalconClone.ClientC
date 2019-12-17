using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class MapGenerated : Signal<MapContent>
    {
    }

    public class MapContent
    {
        public MapUnit[] Map { get; set; }
    }

    public enum MapUnitType
    {
        SMALL = 1,
        MEDIUM = 2,
        BIG = 3,
        BIGGEST = 4
    }

    public class MapPoint
    {
        public float X { get; set; }

        public float Y { get; set; }
    }

    public class MapUnit
    {
        public MapUnitType Type { get; set; }

        [JsonProperty(PropertyName = "units_count")]
        public int UnitsCount { get; set; }

        public int? Owner { get; set; }

        public MapPoint Coords { get; set; }

        public int Id { get; set; }
    }
}
