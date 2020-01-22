using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class PlayerTable
    {
        private static Color[] _colors = new[]
        {
            Color.red, Color.green, Color.blue, Color.magenta
        };

        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public Player Add(int id)
        {
            if (_players.TryGetValue(id, out var player))
                return player;

            player = new Player(id, AssignColor(id));

            _players[id] = player;

            return player;
        }

        private Color AssignColor(int id)
        {
            return _colors[id % _colors.Length];
        }

        public Player getPlayersById(int id)
        {
            return _players.FirstOrDefault(x => x.Key == id).Value;
        }
    }
}
