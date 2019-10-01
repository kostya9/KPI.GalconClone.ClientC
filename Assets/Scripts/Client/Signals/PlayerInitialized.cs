using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class PlayerInitialized : Signal<PlayerInitializedArgs>
    {
    }

    public class PlayerInitializedArgs
    {
        public int Id { get; set; }
        public Player[] Players { get; set; }
    }
}
