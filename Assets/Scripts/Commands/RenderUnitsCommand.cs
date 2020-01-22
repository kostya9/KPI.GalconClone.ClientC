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
                UnitsStore.SetUnitLayout(unitsLayout);
            }

            PlanetLayout planetsLayout = PlanetsStore.GetPlanetLayout();
            int? attackGoalId = planetsLayout.attackGoalId;

            Dictionary<string, int[]> units = Units.unitsAndPlanetsIds;

            bool IsCurrentPlayerUnits = false;

            var added = new List<Unit>();
            foreach(KeyValuePair<string, int[]> kvp in units)
            {
                Planet ownerPlanet = planetsLayout.Find(System.Convert.ToInt32(kvp.Key));
                if (ownerPlanet == null)
                {
                    Debug.LogError("Error: planet hasn't been found");
                }

                double currentAngle = 0;
                foreach (int unit_id in kvp.Value)
                {
                    //Debug.Log("Created unit id: " + unit_id);
                    Vector2 ownerPlanetClientCoords = Translator.ToClient(ownerPlanet.Position);
                    Unit newUnit = new Unit(unit_id, ownerPlanet.Owner, ownerPlanetClientCoords);
                    newUnit.setRoundPosition(currentAngle, Constants.UNIT_RADIUS, ownerPlanetClientCoords);
                    unitsLayout.addUnit(unit_id, newUnit);
                    added.Add(newUnit);
                    currentAngle += Constants.UNIT_ANGLE_IN_RADIANS;
                }

                if (ownerPlanet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                {
                    IsCurrentPlayerUnits = true;
                }

                ownerPlanet.UnitsCount -= kvp.Value.Length;
            }

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
            foreach (var unit in added)
            {
                var copy = GameObject.Instantiate(triangleBlueprint, gameTransform);
                copy.name = "Unit" + unit.Id;
                var transform = (copy.transform as RectTransform);
                var pos = unit.Position;
                //var pos = new Vector2(unitKeyValue.Value.Position.x + rand.Next(100), unitKeyValue.Value.Position.y);
                transform.position = VectorHelper.To2DWorldPosition(pos);
                transform.sizeDelta = new Vector2(15, 15);
                var script = copy.GetComponent<UnitView>();
                if(script == null)
                {
                    Debug.LogError("TriangleBlueprint script is null");
                }
                else
                {
                    script.SetUnit(unit);
                    script.enabled = true;
                    copy.SetActive(true);
                }
            }

            if (IsCurrentPlayerUnits)
            {
                Planet destinationPlanet = planetsLayout.getAttackPlanet();
                foreach (var unit in added)
                {
                    if (unit.IsPlacedOnScene == false)
                    {
                        unit.Destination = destinationPlanet;
                        unit.DestinationPos = Translator.ToClient(destinationPlanet.Position);
                        unit.IsPlacedOnScene = true;
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
