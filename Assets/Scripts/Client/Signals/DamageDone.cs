using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class DamageDone : Signal<DamageDoneArgs>
    {
    }

    public class DamageDoneArgs
    {
        [JsonProperty(PropertyName = "planet_change")]
        public PlanetChange PlanetChange { get; set; }

        [JsonProperty(PropertyName = "unit_id")]
        public int UnitId { get; set; }
    }

    public class PlanetChange
    {
        [JsonProperty(PropertyName = "id")]
        public int PlanetId { get; set; }

        [JsonProperty(PropertyName = "units_count")]
        public int UnitsCount { get; set; }

        public int? Owner { get; set; }

    }
}
