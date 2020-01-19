using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class PlanetLayout : IEnumerable<Planet>
    {
        private readonly Dictionary<int, Planet> _planets;
        public bool startAttackFlag { get; set; }
        public int? attackGoalId { get; set; } // bad variable

        public PlanetLayout(IEnumerable<Planet> planets)
        {
            _planets = planets.ToDictionary(p => p.Id);
            startAttackFlag = false;
        }

        public List<int> getSelectedIds()
        {
            List<int> planetIds = new List<int>();
            foreach (var planet in _planets.Values)
            {
                if (planet.Selected == true)
                {
                    planetIds.Add(planet.Id);
                }
            }
            return planetIds;
        }

        public void SelectSingle(int id)
        {
            foreach (var planet in _planets.Values)
            {
                if (planet.Id == id)
                {
                    if (null != planet.Owner)
                    {
                        if (planet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                        {
                            planet.Selected = true;
                            break;
                        }
                    }
                    startAttackFlag = true;
                    attackGoalId = id;
                    break;
                }
            }
        }
        
        public void SelectMultiple(int id)
        {
            var planet = Find(id);
            if (null != planet.Owner)
            {
                if (planet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                {
                    planet.Selected = !planet.Selected;
                }
            }
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