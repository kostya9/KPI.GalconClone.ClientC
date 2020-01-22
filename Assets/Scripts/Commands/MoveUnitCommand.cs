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

        [Inject]
        public ServerClient client { get; set; }

        public override void Execute()
        {
            Vector2 newPosition = new Vector2(args.X, args.Y);

            GameObject obj = GameObject.Find("Unit" + args.UnitId);
            if (obj != null)
            {
                UnitView uv = obj.GetComponent<UnitView>();
                uv.Move(newPosition);

                // damage condition
                Unit currentUnit = uv.GetUnit();
                if (currentUnit.Destination != null)
                {
                    if (currentUnit.checkCollision())
                    {
                        client.SendDamage(currentUnit.Destination.Id, currentUnit.Id);
                    }
                }
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
                    Vector2 newPosition = unitKeyValue.Value.getNewPosition();
                    Client.SendMove(unitKeyValue.Value.Id, newPosition.x, newPosition.y);
                    Debug.Log("Given: Unit: " + unitKeyValue.Value.Id + ", old Position: " + unitKeyValue.Value.Position + ", new Position: " + newPosition);
                }
            }
        }
    }

    public class MoveUnitInitiated : Signal
    {
    }
}
