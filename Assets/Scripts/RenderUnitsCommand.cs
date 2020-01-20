using Assets.Scripts.Client;
using Assets.Scripts.Players;
using KPI.GalconClone.ClientC;
using Newtonsoft.Json;
using strange.extensions.command.impl;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class RenderUnitsCommand : Command
    {
        [Inject]
        public PlanetSelectedArgs Units { get; set; }

        [Inject]
        public PlanetLayoutStore PlanetsStore { get; set; }

        [Inject]
        public UnitLayoutStore UnitsStore { get; set; }

        private const string Path = "Prefabs/Game";
        private const string TriangleObjectName = "Triangle";

        public override void Execute()
        {
            UnitLayout unitsLayout;
            unitsLayout = UnitsStore.GetUnitLayout();
            if (unitsLayout == null)
            {
                unitsLayout = new UnitLayout();
            }

            PlanetLayout planetsLayout = PlanetsStore.GetPlanetLayout();
            int? attackGoalId = planetsLayout.attackGoalId;

            Dictionary<string, int[]> units = Units.unitsAndPlanetsIds;

            foreach(KeyValuePair<string, int[]> kvp in units)
            {
                Planet ownerPlanet = planetsLayout.Find(System.Convert.ToInt32(kvp.Key));
                if (ownerPlanet == null)
                {
                    Debug.LogError("Error: planet hasn't been found");
                }
                foreach (int unit_id in kvp.Value)
                {
                    //Debug.Log("Created unit id: " + unit_id);
                    unitsLayout.addUnit(unit_id, new Unit(unit_id, ownerPlanet.Owner, ownerPlanet.Position));
                }
            }
            
            UnitsStore.SetUnitLayout(unitsLayout);

            // creating triangle
            GameObject[] objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            GameObject triangleBlueprint = null;
            foreach (GameObject go in objs) {
                if(go.name == "Triangle")
                {
                    triangleBlueprint = go;
                    triangleBlueprint.SetActive(true);
                    break;
                }
            }
            if (triangleBlueprint == null)
                Debug.LogError("Error: triangleBlueprint is null");
            GameObject game = GetGameGO();

            var gameTransform = game.transform;

            var rand = new System.Random();
            foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
            {
                var copy = GameObject.Instantiate(triangleBlueprint, gameTransform); //, spaceTransform
                copy.name = "Unit" + unitKeyValue.Key;
                var transform = (copy.transform as RectTransform);
                copy.transform.position = new Vector2(unitKeyValue.Value.Position.x + rand.Next(100), unitKeyValue.Value.Position.y);
                transform.sizeDelta = new Vector2(100, 100);
                var script = copy.GetComponent<UnitView>();
                if(script == null)
                {
                    Debug.LogError("TriangleBlueprint script is null");
                }
                else
                {
                    script.SetUnit(unitKeyValue.Value);
                    script.enabled = true;
                    copy.SetActive(true);
                }
            }
            
            foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
            {
                GameObject obj = GameObject.Find("Unit" + unitKeyValue.Key);
                if (obj != null)
                {
                    Debug.Log(obj.name + " found " + obj.transform.position);
                }
                else
                {
                    Debug.LogError("Obj name is null");
                }
            }

            //GameObject.Destroy(space);

            //triangleBlueprint.SetActive(false);
            /*
            GameObject game = GameObject.Find("Game");
            Canvas canvas = game.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            */
        }

        private GameObject GetGameGO()
        {
            return UnityEngine.SceneManagement.SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .First(go => go.name.StartsWith("Game"));
        }
    }
}
