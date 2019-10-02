using System;

namespace KPI.GalconClone.ClientC
{
    public class Planet
    {
        public Planet(float positionX, float positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; }
        
        public float PositionX { get; }
        
        public float PositionY { get; }

        public bool Selected { get; set; }
    }
}