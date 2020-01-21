using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class UnitLayout
    {
        public Dictionary<int, Unit> _units { get; }

        public UnitLayout()
        {
            _units = new Dictionary<int, Unit>();
        }

        public void addUnit(int unit_id, Unit unit)
        {
            _units.Add(unit_id, unit);
        }
    }
}
