using Assets.Scripts.Client;
using System;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class Planet
    {
        public Planet(int id, int? ownerId, Vector2 position, Assets.Scripts.Client.MapUnitType type, int unitsCount)
        {
            Position = position;
            Type = type;
            UnitsCount = unitsCount;
            Id = id;
            OwnerId = ownerId;
        }
        
        public int Id { get; }

        public int? OwnerId { get; }

        public Vector2 Position { get; }
        public MapUnitType Type { get; }
        public int UnitsCount { get; }
        public bool Selected { get; set; }
    }
}