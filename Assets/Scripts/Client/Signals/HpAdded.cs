using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class HpAdded : Signal<HpAddedArgs>
    {
    }

    public class HpAddedArgs
    {
        [JsonProperty(PropertyName = "planet_id")]
        public int PlanetId { get; set; }

        [JsonProperty(PropertyName = "hp_count")]
        public int HpCount { get; set; }
    }
}
