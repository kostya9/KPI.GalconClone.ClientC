using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KPI.GalconClone.ClientC
{
    public class SpaceView : BaseView, IPointerClickHandler
    {
        [Inject]
        public PlanetLayoutStore Store { get; set; }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Store.GetPlanetLayout().UnselectAll();
        }
    }
}