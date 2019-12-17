using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class PlanetLayout : IEnumerable<Planet>
    {
        private readonly Dictionary<int, Planet> _planets;

        public PlanetLayout(IEnumerable<Planet> planets)
        {
            _planets = planets.ToDictionary(p => p.Id);
        }

        public void SelectSingle(int id)
        {
            foreach (var planet in _planets.Values)
            {
                planet.Selected = planet.Id == id;
            }
        }
        
        public void SelectMultiple(int id)
        {
            var planet = Find(id); 
            planet.Selected = !planet.Selected;
        }

        public void UnselectAll()
        {
            foreach (var planet in _planets.Values)
            {
                planet.Selected = false;
            }
        }

        public Planet Find(int id)
        {
            if (_planets.TryGetValue(id, out var planet))
                return planet;

            return null;
        }

        public IEnumerator<Planet> GetEnumerator()
        {
            return _planets.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}