using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using strange.extensions.command.impl;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class RenderMapCommand : Command
    {
        private const string Path = "Prefabs/Game";

        private const string PlanetObjectName = "Planet";

        [Inject]
        public MapContent Map { get; set; }

        [Inject]
        public PlanetLayoutStore Store {get;set;}

        public override void Execute()
        {
            Debug.Log("Loading game...");

            var planets = Map.Map
                .Select(u => new Planet(u.Id, u.Owner, new Vector2(u.Coords.X, u.Coords.Y), u.Type, u.UnitsCount));

            var layout = new PlanetLayout(planets);
            Store.SetPlanetLayout(layout);

            var gameResource = Resources.Load<GameObject>(Path);
            if (gameResource is null)
            {
                Debug.LogError("Expected game to be a game object, but got null");
                Fail();
                return;
            }

            var game = GameObject.Instantiate(gameResource);
            var planetBlueprint = game.GetDirectChildByName(PlanetObjectName);

            if (planetBlueprint is null)
            {
                Debug.LogError("Expected planet to be a child game object to the Game prefab");
                Fail();
                return;
            }

            var blueprintRectTransform = planetBlueprint.GetComponent<RectTransform>();

            var gameCanvas = game.GetComponent<Canvas>();

            if (gameCanvas is null)
            {
                Debug.LogError("Expected camera to exist");
                Fail();
                return;
            }

            var serverScreen = new Vector2(1920, 960);
            var clientScreen = new Vector2(gameCanvas.pixelRect.width, gameCanvas.pixelRect.height);
            var delta = serverScreen / 2;
            var scaleFactor = clientScreen / serverScreen;
            var planetWidth = clientScreen.y / 15;

            foreach (var planet in layout)
            {
                var copy = GameObject.Instantiate(planetBlueprint, game.transform);
                var transform = (copy.transform as RectTransform);
                transform.position = scaleFactor * (planet.Position + delta);
                transform.sizeDelta = new Vector2(planetWidth, planetWidth);
                copy.SetActive(true);
                var script = copy.GetComponent<PlanetView>();
                script.Id = planet.Id;
            }

            GameObject.Destroy(planetBlueprint);

            Debug.Log("Game loaded.");
        }
    }
}
