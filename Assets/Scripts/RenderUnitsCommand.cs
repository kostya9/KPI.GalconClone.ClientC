using Assets.Scripts.Client;
using Assets.Scripts.Players;
using KPI.GalconClone.ClientC;
using Newtonsoft.Json;
using strange.extensions.command.impl;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Assets.Scripts.GuiExtensions;

namespace Assets.Scripts
{
    class RenderUnitsCommand : Command
    {
        [Inject]
        public ServerToClientCoordinateTranslator Translator { get; set; }

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

            bool IsCurrentPlayerUnits = false;

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
                    var coords = Translator.ToClient(ownerPlanet.Position);
                    unitsLayout.addUnit(unit_id, new Unit(unit_id, ownerPlanet.Owner, coords));
                }

                if (ownerPlanet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                {
                    IsCurrentPlayerUnits = true;
                }

                ownerPlanet.UnitsCount -= kvp.Value.Length;
            }
            
            UnitsStore.SetUnitLayout(unitsLayout);

            // creating triangle
            GameObject[] objs = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            GameObject triangleBlueprint = null;
            foreach (GameObject go in objs) {
                if(go.name == TriangleObjectName)
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
                var copy = GameObject.Instantiate(triangleBlueprint, gameTransform);
                copy.name = "Unit" + unitKeyValue.Key;
                var transform = (copy.transform as RectTransform);
                var pos = new Vector2(unitKeyValue.Value.Position.x + rand.Next(100), unitKeyValue.Value.Position.y);
                transform.position = VectorHelper.To2DWorldPosition(pos);
                transform.sizeDelta = new Vector2(15, 15);
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

            if (IsCurrentPlayerUnits)
            {
                Planet destinationPlanet = planetsLayout.getAttackPlanet();
                foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
                {
                    if (unitKeyValue.Value.IsPlacedOnScene == false)
                    {
                        unitKeyValue.Value.Destination = destinationPlanet;
                        unitKeyValue.Value.IsPlacedOnScene = true;
                    }
                }
            }
            
            /*
            foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
            {
                GameObject obj = GameObject.Find("Unit" + unitKeyValue.Key);
                if (obj != null)
                {
                    Debug.Log("Unit found: " + obj.name + ", its position: " + obj.transform.position);
                    Unit unit = obj.GetComponent<UnitView>().GetUnit();
                    if (unit.Destination != null)
                    {
                        Debug.Log("Unit destination: " + unit.Destination.Position);
                    }
                }
                else
                {
                    Debug.LogError("Obj name is null");
                }
            }
            */

            triangleBlueprint.SetActive(false);
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
