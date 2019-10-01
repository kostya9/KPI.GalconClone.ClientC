using strange.extensions.signal.impl;

namespace Assets.Scripts.Client
{
    public class GameOver : Signal<GameOverArgs>
    {
    }

    public class GameOverArgs
    {
        public int Winner { get; set; }
    }
}
