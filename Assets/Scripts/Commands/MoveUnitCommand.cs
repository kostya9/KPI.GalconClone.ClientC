using System.Collections.Generic;
using Assets.Scripts.Client;
using strange.extensions.command.impl;
using KPI.GalconClone.ClientC;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Assets.Scripts
{
    class MoveUnitCommand : Command
    {
        [Inject]
        public UnitMovedArgs args { get; }

        [Inject]
        public UnitLayoutStore UnitsStore { get; set; }

        public override void Execute()
        {
            // args.X, args.Y, args.UnitId
        }
    }

    public class InitiateMoveUnits : Command
    {
        [Inject]
        public UnitLayoutStore UnitsStore { get; set; }

        [Inject]
        public ServerClient Client { get; set; }

        public override void Execute()
        {
            var unitsLayout = UnitsStore.GetUnitLayout();
            foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
            {
                if (unitKeyValue.Value.Owner != null && unitKeyValue.Value.Owner.Id == Constants.CURRENT_PLAYER_ID)
                {
                    Debug.Log("Here1");
                    GameObject obj = GameObject.Find("Unit" + unitKeyValue.Key);
                    Debug.Log("Here2");
                    if (obj != null)
                    {
                        Debug.Log("Not null");
                        UnitView uv = obj.GetComponent<UnitView>();
                        uv.Move();
                        Unit movingUnit = uv.GetUnit();
                        Client.SendMove(movingUnit.Id, movingUnit.Position.x, movingUnit.Position.y);
                    }
                    else
                    {
                        Debug.LogError("Error: obj is null");
                    }
                    //unit.Value.Move();
                    Debug.Log("Unit: " + unitKeyValue.Value.Id + ", new Position: " + unitKeyValue.Value.Position);
                }
            }
        }
    }

    public class MoveUnitInitiated : Signal
    {
    }
}
