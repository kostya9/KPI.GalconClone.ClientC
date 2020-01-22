using Assets.Scripts;
using Assets.Scripts.Planets;
using JetBrains.Annotations;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class Planet
    {
        public Planet(int id, [CanBeNull]Player owner, Vector2 position, PlanetType type, int unitsCount)
        {
            Position = position;
            Type = type;
            UnitsCount = unitsCount;
            Id = id;
            Owner = owner;
        }
        
        public int Id { get; }

        [CanBeNull]
        public Player Owner { get; set; }

        public Vector2 Position { get; }
        public PlanetType Type { get; }
        public int UnitsCount { get; set; }
        public bool Selected { get; set; }
    }
}