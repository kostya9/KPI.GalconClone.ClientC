﻿using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.GuiExtensions;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

namespace KPI.GalconClone.ClientC
{
    public class UnitView : BaseView
    {
        private Unit _unit;
        private float _elapsedTime;
        private Vector3 _startingPos;
        private static float TOTAL_MOVE_TIME = Constants.MOVE_INTERVAL / 1000;
        private Vector3 _targetPos;
        private UnityEngine.UI.Image _image;

        public void SetUnit(Unit unit)
        {
            _startingPos = VectorHelper.To2DWorldPosition(unit.Position);
            _targetPos = VectorHelper.To2DWorldPosition(unit.Position);
            _image = GetComponent<Image>();
            _unit = unit;
        }

        public Unit GetUnit()
        {
            return _unit;
        }

        private void Update()
        {
            var owner = _unit.Owner;
            if (owner != null)
            {
                if (owner.Color != _image.color)
                {
                    _image.color = owner.Color;
                }
            }
        }

        private void FixedUpdate()
        {
            _elapsedTime += Time.fixedDeltaTime;
            var t = _elapsedTime / TOTAL_MOVE_TIME;
            transform.position = Vector3.Lerp(_startingPos, _targetPos, t);
        }

        public void Move(Vector2 newPosition)
        {
            _elapsedTime = 0;
            _unit.Position = newPosition;
            _startingPos = transform.position;
            _targetPos = VectorHelper.To2DWorldPosition(newPosition);
        }
    }
}
