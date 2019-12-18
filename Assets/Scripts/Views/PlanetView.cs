using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace KPI.GalconClone.ClientC
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlanetView : BaseView, IPointerClickHandler
    {
        public Planet Planet { get; set; }

        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private bool _uiSelected;

        private void SetupCircle(LineRenderer lineRenderer, float lineWidth = 3, int vertexCount = 100)
        {
            lineRenderer.loop = true;
            var t = (transform as RectTransform);
            var radius = t.sizeDelta.x / 2;
            lineRenderer.widthMultiplier = lineWidth;

            float deltaTheta = (2f * Mathf.PI) / vertexCount;
            float theta = 0f;

            lineRenderer.positionCount = vertexCount;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), -1f);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }

            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            lineRenderer.material = whiteDiffuseMat;
        }


        protected override void Start()
        {
            base.Start();

            var rectTransform = transform as RectTransform;
            var newSize = GetSize(Planet, rectTransform.sizeDelta.x);
            rectTransform.sizeDelta = new Vector2(newSize, newSize);

            var lineRenderer = GetComponent<LineRenderer>();
            SetupCircle(lineRenderer);
            lineRenderer.enabled = false;
        }

        private float GetSize(Planet planet, float initialSize)
        {
            switch (planet.Type)
            {
                case Assets.Scripts.Planets.PlanetType.MEDIUM:
                    return initialSize * 1.2f;
                case Assets.Scripts.Planets.PlanetType.BIG:
                    return initialSize * 1.5f;
                case Assets.Scripts.Planets.PlanetType.BIGGEST:
                    return initialSize * 1.7f;
                case Assets.Scripts.Planets.PlanetType.SMALL:
                default:
                    return initialSize;
            }
        }

        private void Update()
        {
            var imageComponent = GetComponent<Image>();

            var owner = Planet.Owner;
            if (owner != null)
            {
                if(owner.Color != imageComponent.color)
                {
                    Debug.Log("Setting player color");
                    imageComponent.color = owner.Color;
                }
            }

            if (_uiSelected != Planet.Selected)
            {
                var lineRenderer = GetComponent<LineRenderer>();
                
                if (Planet.Selected)
                {
                    lineRenderer.enabled = true;
                }
                else
                {
                    lineRenderer.enabled = false;
                }

                _uiSelected = Planet.Selected;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Store.GetPlanetLayout().SelectMultiple(Planet.Id);
            }
            else
            {
                Store.GetPlanetLayout().SelectSingle(Planet.Id);
            }
        }
    }
}