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
        private const string TriangleObjectName = "Triangle";

        [Inject]
        public ServerToClientCoordinateTranslator Translator { get; set; }

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

            Translator.ClientResolution = new Vector2(gameCanvas.pixelRect.width, gameCanvas.pixelRect.height);

            var planetWidth = Translator.ClientResolution.y / 12;

            foreach (var planet in layout)
            {
                var copy = GameObject.Instantiate(planetBlueprint, game.transform);
                var transform = (copy.transform as RectTransform);
                transform.position = Translator.ToClient(planet.Position);
                transform.sizeDelta = new Vector2(planetWidth, planetWidth);
                var script = copy.GetComponent<PlanetView>();
                script.SetPlanet(planet);
                script.enabled = true;
                copy.SetActive(true);
            }

            GameObject triangleBlueprint = game.GetDirectChildByName(TriangleObjectName);
            triangleBlueprint.SetActive(false);

            GameObject.Destroy(planetBlueprint);
            gameCanvas.worldCamera = Camera.main;

            var menu = GetMenuGO();
            GameObject.Destroy(menu);

            Debug.Log("Game loaded.");
        }

        private GameObject GetMenuGO()
        {
            return UnityEngine.SceneManagement.SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .First(go => go.name.StartsWith("Menu"));
        }
    }
}
