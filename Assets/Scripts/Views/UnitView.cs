using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.GuiExtensions;

namespace KPI.GalconClone.ClientC
{
    public class UnitView : BaseView
    {
        private Unit _unit;

        protected override void Start()
        {
            base.Start();
        }

        public void SetUnit(Unit unit)
        {
            _unit = unit;
        }

        public Unit GetUnit()
        {
            return _unit;
        }

        private void Update()
        {
            var imageComponent = GetComponent<UnityEngine.UI.Image>();

            var owner = _unit.Owner;
            if (owner != null)
            {
                if (owner.Color != imageComponent.color)
                {
                    imageComponent.color = owner.Color;
                }
            }
        }

        public void Move(Vector2 newPosition)
        {
            _unit.Position = newPosition;
            this.transform.position = _unit.Position;
        }
    }
}
