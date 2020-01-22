using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using strange.extensions.command.impl;
using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using strange.extensions.signal.impl;
using Assets.Scripts.Players;

namespace Assets.Scripts
{
    class DamagePlanetCommand : Command
    {
        [Inject]
        public DamageDoneArgs args { get; set; }

        [Inject]
        public PlanetLayoutStore PlanetsStore { get; set; }

        [Inject]
        public PlayerTable Players { get; set; }

        public override void Execute()
        {
            Planet target = PlanetsStore.GetPlanetLayout().Find(args.PlanetChange.PlanetId);
            target.UnitsCount = args.PlanetChange.UnitsCount;
            if (args.PlanetChange.Owner != null)
            {
                target.Owner = Players.getPlayersById((int)args.PlanetChange.Owner);
            }
        }
    }
}
