using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Client
{
    public class PlayerConnected : Signal<PlayerConnectedArgs>
    {
    }

    public class Player
    {
        public int Id { get; set; }

        public bool Ready { get; set; }
    }

    public class PlayerConnectedArgs
    {
        public Player Player { get; set; }
    }
}
