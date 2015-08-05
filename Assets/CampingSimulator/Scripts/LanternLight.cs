using System;
using System.Linq;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
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

        /// <summary>
        /// The minimum brightness of the light when the battery is low 0-1f
        /// </summary>
        [SerializeField] private float MinimumBrightness = 0.25f;
        private float _currentCharge; 

        private Light _lightSource;

        private Color _originalLightBulbMaterialColor, _originalLightColor;
        private Color _currentLightBulbMaterialColor, _targetLightBulbMaterialColor;

        private Color _currentLightColor, _targetLightColor;

        private bool _stateChanging, _isOn = true;
        private Material _lightMaterial;

        private Material GetLightMaterial()
        {
            var meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            return meshRenderer.materials.FirstOrDefault(x => x.name == "Lightbulb (Instance)");
        }

        void Start()
        {
            _lightSource = gameObject.GetComponentInChildren<Light>();
            _currentCharge = MaxCharge;
            _lightMaterial = GetLightMaterial();

            _currentLightBulbMaterialColor = _originalLightBulbMaterialColor = _lightMaterial.GetColor("_EmissionColor");
            _currentLightColor = _originalLightColor = _lightSource.color;

            if (_currentCharge > 0 || UnlimitedBattery)
                TurnOn();
        }

        void Update()
        {
            if (IsOn)
            {
              _targetLightColor = Color.Lerp(new Color(0, 0, 0, 0), _originalLightColor, Mathf.Max(Charge, MinimumBrightness));
              _lightSource.color = _currentLightColor = _targetLightColor;
            }

            if (_stateChanging)
                TransitionLightState();

            _currentLightBulbMaterialColor = Color.Lerp(_currentLightBulbMaterialColor, _targetLightBulbMaterialColor, Time.fixedDeltaTime*10);
            _lightMaterial.SetColor("_EmissionColor", _currentLightBulbMaterialColor);

            UpdateCharge();
        }

        private void UpdateCharge()
        {
            if (UnlimitedBattery || !IsOn ||  _currentCharge <= 0)
                return;

            Debug.Log(String.Format("Lantern Battery: {0:F2}%", Charge * 100));
            _currentCharge -= Time.fixedDeltaTime;

            if (_currentCharge <= 0)
            {
                TurnOff();
                _currentCharge = 0;

                Debug.Log("Lantern out of battery");
            }
        }

        private void TransitionLightState()
        {
            if (_currentLightColor == _targetLightColor)
            {
                _currentLightColor = _targetLightColor;
                _currentLightBulbMaterialColor = _targetLightBulbMaterialColor;
                _stateChanging = false;
            }
            else 
            {
                _currentLightColor = Color.Lerp(_currentLightColor, _targetLightColor, Time.fixedDeltaTime * 10);
                _lightSource.color = _currentLightColor;
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
            get { return _isOn; }
        }

        public void TurnOn()
        {
            if (_currentCharge <= 0 && !UnlimitedBattery)
                return;

            _stateChanging = true;
            _isOn = true;
            _targetLightBulbMaterialColor = _originalLightBulbMaterialColor;
            _targetLightColor = Color.Lerp(new Color(0, 0, 0, 0), _originalLightColor, Mathf.Max(Charge, MinimumBrightness));
        }

        public void TurnOff()
        {
            _stateChanging = true;
            _isOn = false;
            _targetLightBulbMaterialColor = new Color(0, 0, 0, 0);
            _targetLightColor = new Color(0, 0, 0, 0);
        }

        /// <summary>
        /// How much charge is left in the light's power source. 0-1f
        /// </summary>
        public float Charge
        {
            get { return (_currentCharge/MaxCharge); }
        }
    }
}