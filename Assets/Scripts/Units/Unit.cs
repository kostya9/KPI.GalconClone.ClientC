using Assets.Scripts;
using Assets.Scripts.Planets;
using JetBrains.Annotations;
using UnityEngine;
using KPI.GalconClone.ClientC;
using Assets.Scripts.GuiExtensions;
using System;

public class Unit
{
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

    private Vector3 GetDestinationPosition()
    {
        return VectorHelper.To2DWorldPosition(this.Destination.Position);
    }

    public Vector2 getNewPosition()
    {
        Vector3 destinationPos = this.GetDestinationPosition();
        float distanceX = destinationPos.x - this.Position.x;
        float distanceY = destinationPos.y - this.Position.y;
        float distance = (float)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
        float ratio = distance / Constants.UNIT_SPEED;
        float deltaX = distanceX / ratio;
        float deltaY = distanceY / ratio;
        return new Vector2(Position.x + deltaX, Position.y + deltaY);
    }
}