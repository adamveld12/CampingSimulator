using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    [RequireComponent(typeof (AudioSource))]
    public class FlashlightBehavior : MonoBehaviour, IInteractable
    {
        [SerializeField]
        [Header("Flashlight Attributes")]
        [Tooltip("The dimmest this light can get")]
        public float MinimumLightBrightness = 0.2f;

        [SerializeField]
        [Tooltip("The flashlight duration (in seconds)")]
        public float MaxLightCharge = 20f;

        [SerializeField]
        [Tooltip("The amount of charge the flashlight gains per second of shaking (seconds per second?)")]
        public float ChargePerSecond = 1.0f;

        [SerializeField]
        [Tooltip("Toggles unlimited flashlight battery")]
        public bool UnlimitedBattery = false;

        [SerializeField]
        [Tooltip("Controls the light's state")]
        public bool IsLightOn;

        private Light _lightSource;
        private float _currentCharge;
        private Color _originalLightColor;

        private Material _lightMaterial;

        private AudioClip _onClip, _offClip;
        private AudioSource _source;

        void Start()
        {
            _source = GetComponent<AudioSource>();

            _onClip = AssetDatabase.LoadAssetAtPath<AudioClip>(@"Assets/CampingSimulator/Prefabs/Flashlight/on.ogg");
            _offClip = AssetDatabase.LoadAssetAtPath<AudioClip>(@"Assets/CampingSimulator/Prefabs/Flashlight/off.ogg");
            
            Debug.Assert(_onClip != null, "on clip is null");
            Debug.Assert(_offClip != null);

            IsOn = IsLightOn;
            _currentCharge = MaxLightCharge;
            _lightSource = GetComponentInChildren<Light>();

            if (_lightSource == null)
                throw new MissingComponentException("Missing a light component on one of the children Game Objects");


            _originalLightColor = _lightSource.color;
            _lightMaterial = gameObject.GetComponentsInChildren<MeshRenderer>().SelectMany(x => x.materials).FirstOrDefault(x => x.name == "Lightbulb (Instance)");
        }

        void Update()
        {
            if (UnlimitedBattery || !IsOn)
                return;

            if (IsOn && HasCharge)
            {
              _lightSource.color = Color.Lerp(new Color(0, 0, 0, 0), _originalLightColor, GetLightModifier());
              _currentCharge = Mathf.Max(_currentCharge - Time.fixedDeltaTime, 0);
              Debug.Log(string.Format("{0:F}%", Charge * 100));
            } 
            else if (!HasCharge)
            {
                _currentCharge = 0;
                IsOn = false;
            }
        }

        private float GetLightModifier()
        {
            return Mathf.Lerp(1.0f, MinimumLightBrightness, _currentCharge/MaxLightCharge);
        }

        public void Interact()
        {
            IsOn = !IsOn;
        }

        public string GetInteractText()
        {
            return String.Format("Turn {0}.", IsOn ? "off" : "on");
        }

        public float Charge
        {
            get { return _currentCharge/MaxLightCharge; }
        }

        /// <summary>
        /// If the flashlight has any charge left
        /// </summary>
        public bool HasCharge
        {
            get { return _currentCharge > 0; }
        }

        public bool IsCharging { get; set; }

        /// <summary>
        /// If the flashlight is on or off
        /// </summary>
        public bool IsOn
        {
            get
            {
                return IsLightOn;
            }
            set
            {
                if (!UnlimitedBattery && !HasCharge && value)
                    return;

                 IsLightOn = value;

                if (_lightSource != null)
                {
                   _lightSource.enabled = IsLightOn;
                   _lightMaterial.SetColor("_EmissionColor", IsLightOn ? new Color(4, 4, 4, 1) : new Color(0,0,0,1));
                }

                _source.PlayOneShot(IsLightOn ? _onClip : _offClip);
            }
        }

    }
}