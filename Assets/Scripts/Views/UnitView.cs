using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.GuiExtensions;

namespace KPI.GalconClone.ClientC
{
    [RequireComponent(typeof(LineRenderer))]
    public class UnitView : BaseView
    {
        private Unit _unit;

        protected override void Start()
        {
            base.Start();

            SetupSelectionTriangle();
        }

        private void SetupSelectionTriangle()
        {
            /*
            var lineRenderer = GetComponent<LineRenderer>();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            //whiteDiffuseMat.SetTexture();
            lineRenderer.material = whiteDiffuseMat;
            lineRenderer.enabled = false;
            
            LineRendererHelper.DrawCircle(lineRenderer);
            */
        }

        public void SetUnit(Unit unit)
        {
            _unit = unit;
        }

        private void Update()
        {
        }
    }
}
