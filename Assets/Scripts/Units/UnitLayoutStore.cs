using System;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class UnitLayoutStore
    {
        private UnitLayout _layout;

        public void SetUnitLayout(UnitLayout layout)
        {
            _layout = layout ?? throw new ArgumentNullException(nameof(layout));
        }

        public UnitLayout GetUnitLayout()
        {
            if (_layout == null)
            {
                //throw new InvalidOperationException("The layout was not set, cannot get it");
                Debug.Log("UnitLayoutStore: The layout was not set, cannot get it");
            }

            return _layout;
        }
    }
}