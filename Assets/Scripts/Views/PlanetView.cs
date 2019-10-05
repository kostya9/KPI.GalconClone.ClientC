using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace KPI.GalconClone.ClientC
{
    public class PlanetView : BaseView, IPointerClickHandler
    {
        [SerializeField]
        public Guid Id;

        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private bool _uiSelected;
        
        private void Update()
        {
            var planet = Store.GetPlanetLayout().Find(Id);
            if (_uiSelected != planet.Selected)
            {
                var renderer = GetComponent<Image>();
                if (planet.Selected)
                {
                    renderer.color = Color.blue;
                }
                else
                {
                    renderer.color = Color.white;
                }

                _uiSelected = planet.Selected;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Store.GetPlanetLayout().SelectMultiple(Id);
            }
            else
            {
                Store.GetPlanetLayout().SelectSingle(Id);
            }
        }
    }
}