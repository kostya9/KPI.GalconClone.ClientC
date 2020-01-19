using Assets.Scripts.Client;
using Assets.Scripts.Players;
using KPI.GalconClone.ClientC;
using strange.extensions.command.impl;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class RenderMenuCommand : Command
    {
        private const string Path = "Prefabs/Menu";

        public override void Execute()
        {
            Debug.Log("Loading Menu...");


            var gameResource = Resources.Load<GameObject>(Path);
            if (gameResource is null)
            {
                Debug.LogError("Expected game to be a game object, but got null");
                Fail();
                return;
            }

            var game = GameObject.Instantiate(gameResource);
            

            var gameCanvas = game.GetComponent<Canvas>();

            if (gameCanvas is null)
            {
                Debug.LogError("Expected camera to exist");
                Fail();
                return;
            }

            gameCanvas.worldCamera = Camera.main;

            Debug.Log("Menu loaded.");
        }
    }
}
