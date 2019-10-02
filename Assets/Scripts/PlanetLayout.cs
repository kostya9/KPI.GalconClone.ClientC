using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace KPI.GalconClone.ClientC
{
    public class PlanetLayout
    {
        public float PositionX { get; }
        public float PositionY { get; }

        public PlanetLayout(float positionX, float positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
        
        public static IEnumerable<PlanetLayout> GeneratePlanets(int count, Vector2 size, Vector2 minCoordinates, Vector2 maxCoordinates)
        {
            var random = new Random();
            var generated = new List<PlanetLayout>();
            
            for (int i = 0; i < count; i++)
            {
                yield return CreatePlanet(size, minCoordinates, maxCoordinates, random, generated);
            }
        }

        private static PlanetLayout CreatePlanet(Vector2 size, Vector2 minCoordinates, Vector2 maxCoordinates, Random r, List<PlanetLayout> generated)
        {
            const int maxAttempts = 50;
            for (var attempt = 1; attempt < maxAttempts; attempt++)
            {
                var x = (float) r.NextDouble(minCoordinates.x + size.y / 2, maxCoordinates.x - size.x / 2);
                var y = (float) r.NextDouble(minCoordinates.y + size.y / 2, maxCoordinates.y - size.x / 2);
                var layoutItem = new PlanetLayout(x, y);
                
                if (!IntersectsWithAny(layoutItem, generated, size))
                {
                    generated.Add(layoutItem);
                    return layoutItem;
                }
            }
            
            throw new InvalidOperationException($"Could not generate a position for planet in {maxAttempts} attemts");
        }

        private static bool IntersectsWithAny(PlanetLayout layoutItem, List<PlanetLayout> generated, Vector2 size)
        {
            foreach (var generatedItem in generated)
            {
                if (Math.Abs(layoutItem.PositionX - generatedItem.PositionX) < size.x
                    && Math.Abs(layoutItem.PositionY - generatedItem.PositionY) < size.y)
                    return true;
            }

            return false;
        }
    }
}