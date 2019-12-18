using UnityEngine;

namespace Assets.Scripts
{
    public class Player
    {
        public Player(int id, Color color)
        {
            Id = id;
            Color = color;
        }

        public int Id { get; }

        public Color Color { get; }
    }
}
