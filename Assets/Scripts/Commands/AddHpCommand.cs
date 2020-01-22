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

namespace Assets.Scripts
{
    class AddHpCommand : Command
    {
        [Inject]
        public HpAddedArgs args { get; set; }

        [Inject]
        public PlanetLayoutStore PlanetsStore { get; set; }

        public override void Execute()
        {
            var target = PlanetsStore.GetPlanetLayout().Find(args.PlanetId);
            target.UnitsCount += args.HpCount;
        }
    }

    class InitiateAddHpCommand : Command
    {
        [Inject] 
        public PlanetLayoutStore PlanetStore { get; set; }

        [Inject] 
        public ServerClient Client { get; set; }

        public override void Execute()
        {
            var playerPlanets = PlanetStore.GetPlanetLayout().GetPlanetOfPlayer(Constants.CURRENT_PLAYER_ID);
            foreach (var planet in playerPlanets)
            {
                Client.SendAddHp(planet.Id);
            }
        }
    }

    public class AppHpInitiated : Signal
    {

    }
}
