using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace KPI.GalconClone.ClientC
{
    public class PlanetLayout : IEnumerable<Planet>
    {
        private readonly Dictionary<Guid, Planet> _planets;

        private PlanetLayout(IEnumerable<Planet> planets)
        {
            _planets = planets.ToDictionary(p => p.Id);
        }

        public void SelectSingle(Guid id)
        {
            foreach (var planet in _planets.Values)
            {
                planet.Selected = planet.Id == id;
            }
        }
        
        public void SelectMultiple(Guid id)
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

        public Planet Find(Guid id)
        {
            if (_planets.TryGetValue(id, out var planet))
                return planet;

            return null;
        }
        
        public static PlanetLayout GeneratePlanets(int count, Vector2 size, Vector2 minCoordinates, Vector2 maxCoordinates)
        {
            var random = new Random();
            var generated = new List<Planet>();
            
            for (int i = 0; i < count; i++)
            {
                var planet = CreatePlanet(size, minCoordinates, maxCoordinates, random, generated);
                generated.Add(planet);
            }
            
            return new PlanetLayout(generated);
        }

        private static Planet CreatePlanet(Vector2 size, Vector2 minCoordinates, Vector2 maxCoordinates, Random r, IEnumerable<Planet> generated)
        {
            const int maxAttempts = 50;
            for (var attempt = 1; attempt < maxAttempts; attempt++)
            {
                var x = (float) r.NextDouble(minCoordinates.x + size.y / 2, maxCoordinates.x - size.x / 2);
                var y = (float) r.NextDouble(minCoordinates.y + size.y / 2, maxCoordinates.y - size.x / 2);
                var layoutItem = new Planet(x, y);
                
                if (!IntersectsWithAny(layoutItem, generated, size))
                {
                    return layoutItem;
                }
            }
            
            throw new InvalidOperationException($"Could not generate a position for planet in {maxAttempts} attemts");
        }

        private static bool IntersectsWithAny(Planet planet, IEnumerable<Planet> generated, Vector2 size)
        {
            foreach (var generatedItem in generated)
            {
                if (Math.Abs(planet.PositionX - generatedItem.PositionX) < size.x
                    && Math.Abs(planet.PositionY - generatedItem.PositionY) < size.y)
                    return true;
            }

            return false;
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