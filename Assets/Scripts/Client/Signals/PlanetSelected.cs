using Newtonsoft.Json;
using strange.extensions.signal.impl;
using System.Collections.Generic;

namespace Assets.Scripts.Client
{
    public class PlanetSelected : Signal<PlanetSelectedArgs>
    {
    }

    public class PlanetSelectedArgs
    {
        [JsonProperty(PropertyName = "selected")]
        public Dictionary<string, int[]> unitsAndPlanetsIds { get; set; }
    }
}
