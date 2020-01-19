using Assets.Scripts;
using Assets.Scripts.Planets;
using JetBrains.Annotations;
using UnityEngine;

public class Unit
{
    public Unit(int id, Player owner, Vector2 position)
    {
        Position = position;
        Id = id;
        Owner = owner;
    }

    public int Id { get; }

    public Player Owner { get; }

    public Vector2 Position { get; }
    public PlanetType Type { get; }
    public int UnitsCount { get; }
    public bool Selected { get; set; }
}