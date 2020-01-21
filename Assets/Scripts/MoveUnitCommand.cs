using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Client;
using UnityEngine;
using strange.extensions.command.impl;
using KPI.GalconClone.ClientC;

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
}
