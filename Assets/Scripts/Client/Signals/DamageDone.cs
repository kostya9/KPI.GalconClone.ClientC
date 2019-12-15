using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class DamageDone : Signal<DamageDoneArgs>
    {
    }

    public class DamageDoneArgs
    {
        public PlanetChange PlanetChange { get; set; }
    }

    public class PlanetChange
    {
        [JsonProperty(PropertyName = "planet_id")]
        public int PlanetId { get; set; }

        [JsonProperty(PropertyName = "units_count")]
        public int UnitsCount { get; set; }

        public int? Owner { get; set; }

    }
}
