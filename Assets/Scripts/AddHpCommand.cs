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

namespace Assets.Scripts
{
    class AddHpCommand : Command
    {
        [Inject]
        public HpAddedArgs args { get; }

        [Inject]
        public PlanetLayoutStore PlanetsStore { get; set; }

        public override void Execute()
        {
            PlanetLayout pl = PlanetsStore.GetPlanetLayout();
            foreach (Planet planet in pl)
            {
                if (planet.Id == args.PlanetId)
                {
                    planet.UnitsCount += args.HpCount;
=======
                    break;
                }
            }
        }
    }
}
