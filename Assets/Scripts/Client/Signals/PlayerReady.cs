using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Client
{
    public class PlayerReady : Signal<PlayerReadyArgs>
    {
    }

    public class PlayerReadyArgs
    {
        public int Player { get; set; }

        public bool Ready { get; set; }
    }
}
