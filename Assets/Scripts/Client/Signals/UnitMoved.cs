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

        public double X { get; set; }

        public double Y { get; set; }
    }
}
