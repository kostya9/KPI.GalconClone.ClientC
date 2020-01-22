using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class UnitMoved : Signal<UnitMovedArgs>
    {
    }

    public class UnitMovedArgs
    {
        [JsonProperty(PropertyName = "unit_id")]
        public int UnitId { get; set; }

        [JsonProperty(PropertyName = "x")]
        public float X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public float Y { get; set; }
    }
}
