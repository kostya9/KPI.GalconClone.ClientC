using System.Resources;
using strange.extensions.command.impl;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class StartGameCommand : Command
    {
        private const string Path = "Prefabs/Game";

        private const string PlanetObjectName = "Planet";
        
        [Inject]
        public PlanetLayoutStore LayoutStore { get; set; }
        
        public override void Execute()
        {
            Debug.Log("Loading game...");
            
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

            var size = blueprintRectTransform.rect.width * planetBlueprint.transform.localScale * 1.5f; // Make more space between planets
            var minCoordinates = new Vector2(0, 0);
            var maxCoordinates = new Vector2(gameCanvas.pixelRect.width, gameCanvas.pixelRect.height);
            
            var planetCount = 8;
            var layout = PlanetLayout.GeneratePlanets(planetCount, size, minCoordinates, maxCoordinates);
            LayoutStore.SetPlanetLayout(layout);
            foreach (var planet in layout)
            {
                var copy = GameObject.Instantiate(planetBlueprint, game.transform);
                copy.transform.position = new Vector3(planet.PositionX, planet.PositionY);
                copy.SetActive(true);
                var script = copy.GetComponent<PlanetView>();
                script.Id = planet.Id;
            }
            
            GameObject.Destroy(planetBlueprint);
            
            Debug.Log("Game loaded.");
        }
    }
}