using System;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class SpaceView : BaseView
    {
        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private void OnMouseDown()
        {
            Store.GetPlanetLayout().UnselectAll();
        }
    }
}