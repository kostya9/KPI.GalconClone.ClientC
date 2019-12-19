using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using TMPro;

namespace KPI.GalconClone.ClientC
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlanetView : BaseView, IPointerClickHandler
    {
        public Planet Planet { get; set; }
        
        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private bool _uiSelected;
        private RectTransform _rectTransform;

        protected override void Start()
        {
            base.Start();

            InitSizeAccordingToPlanetType();
            SetupSelectionCircle();
            InitPlanetHealthText();
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

        private void InitPlanetHealthText()
        {
            var textComp = GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = Planet.UnitsCount.ToString();
            textComp.rectTransform.sizeDelta = _rectTransform.sizeDelta;
            textComp.alignment = TextAlignmentOptions.Center;
            textComp.fontSize = _rectTransform.sizeDelta.x / 3;
        }

        private void SetupSelectionCircle(float lineWidth = 3, int vertexCount = 100)
        {
            var lineRenderer = GetComponent<LineRenderer>();

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

            lineRenderer.enabled = false;
        }

        private void InitSizeAccordingToPlanetType()
        {
            _rectTransform = transform as RectTransform;
            var newSize = GetSize(Planet, _rectTransform.sizeDelta.x);
            _rectTransform.sizeDelta = new Vector2(newSize, newSize);
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
    }
}