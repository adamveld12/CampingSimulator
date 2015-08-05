using System;
using System.Linq;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Light))]
    public class LanternLight : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// How many seconds of battery power this light has
        /// </summary>
        [SerializeField] private float MaxCharge = 100.0f;

        /// <summary>
        /// Enables an unlimited battery supply
        /// </summary>
        [SerializeField] private bool UnlimitedBattery = false;
        private float _currentCharge; 

        private Canvas _canvas;
        
        private Light _lightSource;
        private Color _originalLightBulbMaterialColor, _originalLightColor;
        private Color _currentLightBulbMaterialColor, _targetLightBulbMaterialColor;
        private Color _currentLightColor, _targetLightColor;
        private Material _lightMaterial;

        private Material GetLightMaterial()
        {
            var meshRenderer = gameObject.GetComponentInParent<MeshRenderer>();
            return meshRenderer.materials.FirstOrDefault(x => x.name == "Lightbulb (Instance)");
        }

        void Start()
        {
            _canvas = gameObject.GetComponent<Canvas>();
            _lightSource = gameObject.GetComponent<Light>();
            _currentCharge = MaxCharge;
            _lightMaterial = GetLightMaterial();
            _originalLightColor = _lightSource.color;

            _currentLightBulbMaterialColor = _originalLightBulbMaterialColor = _lightMaterial.GetColor("_EmissionColor");

            if (_currentCharge > 0 || UnlimitedBattery)
                TurnOn();
        }

        void Update()
        {
            if (_currentLightBulbMaterialColor != _targetLightBulbMaterialColor)
            {
                _currentLightBulbMaterialColor = Color.Lerp(_currentLightBulbMaterialColor, _targetLightBulbMaterialColor, Time.fixedDeltaTime*10);
                _lightMaterial.SetColor("_EmissionColor", _currentLightBulbMaterialColor);
            }

            if (_currentLightColor != _targetLightColor)
            {
                _currentLightColor = Color.Lerp(_currentLightColor, _targetLightColor, Time.fixedDeltaTime * 10);
                _lightSource.color = _currentLightColor;
            }


            if (_currentLightColor == _targetLightColor)
            {
                _lightSource.enabled = !IsOn;
            }

            if (UnlimitedBattery)
                return;

            if (IsOn)
            {
                _currentCharge -= Time.fixedDeltaTime;
                Debug.Log(String.Format("Lantern Battery: {0}%", Charge));

                if (_currentCharge <= 0)
                {
                  TurnOff();
                  _currentCharge = 0;

                  Debug.Log("Lantern off");
                }
            } 
        }

        public string GetInteractText()
        {
            return String.Format("Turn {0}.", IsOn ? "off" : "on");
        }

        public void Interact()
        {
            if (_currentCharge > 0 || UnlimitedBattery)
            {
                Debug.Log("Toggling light");
                if (IsOn) 
                    TurnOff();
                else 
                    TurnOn();
            }
        }


        public bool IsOn
        {
            get { return _lightSource.enabled; }
        }

        public void TurnOn()
        {
            if (_currentCharge <= 0 && !UnlimitedBattery)
                return;

            _lightSource.enabled = true;
            _targetLightBulbMaterialColor = _originalLightBulbMaterialColor;
            _targetLightColor = _originalLightColor;
        }

        public void TurnOff()
        {
            //_lightSource.enabled = false;
            _targetLightBulbMaterialColor = new Color(0, 0, 0, 0);
            _targetLightColor = new Color(0, 0, 0, 0);
        }

        public float Charge
        {
            get { return (_currentCharge/MaxCharge) * 100; }
        }
    }
}