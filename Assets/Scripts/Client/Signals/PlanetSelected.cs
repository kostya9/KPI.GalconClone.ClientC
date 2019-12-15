using Newtonsoft.Json;
using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class PlanetSelected : Signal<PlanetSelectedArgs>
    {
    }

    public class PlanetSelectedArgs
    {
        public SelectedPlanets Selected { get; set; }
    }

    public class SelectedPlanets
    {
        [JsonProperty(PropertyName = "planet_id")]
        public int[] PlanetIds { get; set; }
    }
}
