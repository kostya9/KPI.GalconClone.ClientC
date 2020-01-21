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
        public UnitMovedArgs args { get; set; }

        [Inject]
        public UnitLayoutStore UnitsStore { get; set; }

        public override void Execute()
        {
            Vector2 newPosition = new Vector2(args.X, args.Y);
            Debug.Log("Got: Unit: " + args.UnitId + ", new Position: " + newPosition);

            GameObject obj = GameObject.Find("Unit" + args.UnitId);
            if (obj != null)
            {
                Debug.Log("Unit obj is not null");
                UnitView uv = obj.GetComponent<UnitView>();
                uv.Move(newPosition);
            }
            else
            {
                Debug.LogError("Error: obj is null");
            }
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
                    Debug.Log("Position: " + unitKeyValue.Value.Position);
                    Vector2 newPosition = unitKeyValue.Value.getNewPosition();
                    Client.SendMove(unitKeyValue.Value.Id, newPosition.x, newPosition.y);
                    Debug.Log("Given: Unit: " + unitKeyValue.Value.Id + ", new Position: " + newPosition);
                }
            }
        }
    }

    public class MoveUnitInitiated : Signal
    {
    }
}
