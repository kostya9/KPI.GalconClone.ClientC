using Assets.Scripts;
using Assets.Scripts.Planets;
using JetBrains.Annotations;
using UnityEngine;
using KPI.GalconClone.ClientC;
using Assets.Scripts.GuiExtensions;
using System;

public class Unit
{
    [Inject]
    public ServerToClientCoordinateTranslator Translator { get; }

    public Unit(int id, Player owner, Vector2 position)
    {
        Position = position;
        Id = id;
        Owner = owner;
        IsPlacedOnScene = false;
    }

    public int Id { get; }

    public Player Owner { get; }

    public Vector2 Position { get; set; }

    [CanBeNull]
    public Planet Destination { get; set; }

    public bool IsPlacedOnScene { get; set; }

    public Vector2 DestinationPos { get; set; }

    public Vector2 getNewPosition()
    {
        Vector3 destinationPos = DestinationPos;
        float distanceX = destinationPos.x - this.Position.x;
        float distanceY = destinationPos.y - this.Position.y;
        float distance = (float)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
        float ratio = distance / Constants.UNIT_SPEED;
        float deltaX = distanceX / ratio;
        float deltaY = distanceY / ratio;
        return new Vector2(Position.x + deltaX, Position.y + deltaY);
    }

    public void setRoundPosition(double angle, float radius, Vector2 ownerPlanetClientCoords)
    {
        float cathetus1 = (float)Math.Sin(angle) * radius;
        float cathetus2 = (float)Math.Cos(angle) * radius;
        this.Position = new Vector2(ownerPlanetClientCoords.x + cathetus1, ownerPlanetClientCoords.y + cathetus2);
    }

    public bool checkCollision()
    {
        return Math.Pow(Position.x - DestinationPos.x, 2) + Math.Pow(Position.y - DestinationPos.y, 2) <= Math.Pow(Constants.PLANET_RADIUS, 2);
    }
}