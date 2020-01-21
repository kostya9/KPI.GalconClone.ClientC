using System;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class PlanetLayoutStore
    {
        private PlanetLayout _layout;

        public void SetPlanetLayout(PlanetLayout layout)
        {
            _layout = layout ?? throw new ArgumentNullException(nameof(layout));
        }

        public PlanetLayout GetPlanetLayout()
        {
            if(_layout == null)
            {
                //throw new InvalidOperationException("The layout was not set, cannot get it");
                Debug.Log("PlanetLayoutStore: The layout was not set, cannot get it");
            }

            return _layout;
        }
    }
}