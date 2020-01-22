using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.GuiExtensions;

namespace KPI.GalconClone.ClientC
{
    public class UnitView : BaseView
    {
        private Unit _unit;
        private float _journeyLength;
        private float _elapsedTime;
        private Vector3 _startingPos;

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

            _elapsedTime += Time.deltaTime;
            var total = Constants.MOVE_INTERVAL / 1000;
            var t = _elapsedTime / total;

            transform.position = Vector3.Lerp(_startingPos, VectorHelper.To2DWorldPosition(_unit.Position), t);
        }

        public void Move(Vector2 newPosition)
        {
            _elapsedTime = 0;
            _unit.Position = newPosition;
            _startingPos = transform.position;
        }
    }
}
