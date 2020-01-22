using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using TMPro;
using Assets.Scripts.GuiExtensions;

namespace KPI.GalconClone.ClientC
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlanetView : BaseView, IPointerClickHandler
    {
        private Planet _planet;
        
        [Inject]
        public PlanetLayoutStore Store { get; set; }

        private bool _uiSelected;
        private int _uiHealth;

        private RectTransform _rectTransform;
        private TextMeshProUGUI _healthText;

        protected override void Start()
        {
            base.Start();

            InitSizeAccordingToPlanetType();
            SetupSelectionCircle();
            InitPlanetHealthText();
        }

        public void SetPlanet(Planet planet)
        {
            _planet = planet;
        }

        private void Update()
        {
            var imageComponent = GetComponent<Image>();

            var owner = _planet.Owner;
            if (owner != null)
            {
                if(owner.Color != imageComponent.color)
                {
                    Debug.Log("Setting player color");
                    imageComponent.color = owner.Color;
                }
            }

            if (_uiSelected != _planet.Selected)
            {
                var lineRenderer = GetComponent<LineRenderer>();
                
                if (_planet.Selected)
                {
                    lineRenderer.enabled = true;
                }
                else
                {
                    lineRenderer.enabled = false;
                }

                _uiSelected = _planet.Selected;
            }

            if(_uiHealth != _planet.UnitsCount)
            {
                _healthText.text = _planet.UnitsCount.ToString();
                _uiHealth = _planet.UnitsCount;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Store.GetPlanetLayout().SelectMultiple(_planet.Id);
            }
            else
            {
                Store.GetPlanetLayout().SelectSingle(_planet.Id);
            }
        }

        private void InitPlanetHealthText()
        {
            _uiHealth = _planet.UnitsCount;

            _healthText = GetComponentInChildren<TextMeshProUGUI>();
            _healthText.text = _uiHealth.ToString();
            _healthText.rectTransform.sizeDelta = _rectTransform.sizeDelta;
            _healthText.alignment = TextAlignmentOptions.Center;
            _healthText.fontSize = _rectTransform.sizeDelta.x / 3;
        }

        private void SetupSelectionCircle()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
            lineRenderer.material = whiteDiffuseMat;
            lineRenderer.enabled = false;

            LineRendererHelper.DrawCircle(lineRenderer, _rectTransform.sizeDelta.x / 2);
        }

        private void InitSizeAccordingToPlanetType()
        {
            _rectTransform = transform as RectTransform;
            var newSize = GetSize(_planet, _rectTransform.sizeDelta.x);
            _rectTransform.sizeDelta = new Vector2(newSize, newSize);
            _planet.Radius = newSize / 2;
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