using System.Resources;
using strange.extensions.command.impl;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class StartGameCommand : Command
    {
        private const string Path = "Prefabs/Game";

        private const string PlanetObjectName = "planet";
        
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

            var spriteRenderer = planetBlueprint.GetComponent<SpriteRenderer>();
            
            var camera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;

            if (camera is null)
            {
                Debug.LogError("Expected camera to exist");
                Fail();
                return;
            }

            var size = spriteRenderer.size * planetBlueprint.transform.localScale * 1.5f; // Make more space between planets
            var minCoordinates = new Vector2(camera.transform.position.x - camera.orthographicSize, camera.transform.position.y - camera.orthographicSize);
            var maxCoordinates = new Vector2(camera.transform.position.x + camera.orthographicSize, camera.transform.position.y + camera.orthographicSize);
            
            var planetCount = 8;
            foreach (var layoutItem in PlanetLayout.GeneratePlanets(planetCount, size, minCoordinates, maxCoordinates))
            {
                var copy = GameObject.Instantiate(planetBlueprint, game.transform);
                copy.transform.position = new Vector3(layoutItem.PositionX, layoutItem.PositionY);
                copy.SetActive(true);
            }
            
            GameObject.Destroy(planetBlueprint);
            
            Debug.Log("Game loaded.");
        }
    }
}