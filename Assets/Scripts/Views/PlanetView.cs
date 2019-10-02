using System;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class PlanetView : BaseView
    {
        [SerializeField]
        public Guid Id;

        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private bool _uiSelected;

        private void OnMouseDown()
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

        private void Update()
        {
            var planet = Store.GetPlanetLayout().Find(Id);
            if (_uiSelected != planet.Selected)
            {
                var renderer = GetComponent<SpriteRenderer>();
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
    }
}