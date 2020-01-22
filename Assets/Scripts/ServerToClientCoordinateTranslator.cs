using UnityEngine;

namespace Assets.Scripts
{
    public class ServerToClientCoordinateTranslator
    {
        private readonly Vector2 _serverResolution = new Vector2(1920, 960); // From server code

        public Vector2 ClientResolution { get; set; }

        public Vector2 ToClient(Vector2 serverCoords)
        {
            var scaleFactor = ClientResolution / _serverResolution;
            var delta = _serverResolution / 2;

            return scaleFactor * (serverCoords + delta);
        }
    }
}
