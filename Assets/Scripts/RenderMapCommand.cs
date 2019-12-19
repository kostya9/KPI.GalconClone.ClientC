using Assets.Scripts.Client;
using Assets.Scripts.Players;
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

        [Inject]
        public PlayerTable Players { get; set; }

        private Planet ToPlanet(MapUnit u)
        {
            var coords = new Vector2(u.Coords.X, u.Coords.Y);

            var owner = u.Owner.HasValue
                ? Players.Add(u.Owner.Value)
                : null;

            return new Planet(u.Id, owner, coords, u.Type, u.UnitsCount);
        }

        public override void Execute()
        {
            Debug.Log("Loading game...");

            var planets = Map.Map
                .Select(ToPlanet);

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
            var planetWidth = clientScreen.y / 12;

            // Add a border
            var scaleFactor = clientScreen / (serverScreen + new Vector2(planetWidth, planetWidth));
            var delta = serverScreen / 2;

            foreach (var planet in layout)
            {
                var copy = GameObject.Instantiate(planetBlueprint, game.transform);
                var transform = (copy.transform as RectTransform);
                transform.position = scaleFactor * (planet.Position + delta);
                transform.sizeDelta = new Vector2(planetWidth, planetWidth);
                var script = copy.GetComponent<PlanetView>();
                script.SetPlanet(planet);
                script.enabled = true;
                copy.SetActive(true);
            }

            GameObject.Destroy(planetBlueprint);

            gameCanvas.worldCamera = Camera.main;

            Debug.Log("Game loaded.");
        }
    }
}
