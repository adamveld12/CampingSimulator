using System;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    public interface IInteractable
    {
        void Interact();
        string GetInteractText();
    }

    [Serializable]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Light))]
    public class LanternLight : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// How many seconds of battery power this light has
        /// </summary>
        [SerializeField] private float maxCharge = 100.0f;

        private float _currentCharge; 
        private Light _lightToggle;

        void Start()
        {
            _lightToggle = gameObject.GetComponent<Light>();
            _currentCharge = maxCharge;
            _lightToggle.enabled = true;
        }

        void Update()
        {
            if (_lightToggle.enabled)
            {
                _currentCharge -= Time.deltaTime;
                Debug.Log(String.Format("Charge is {0}%", maxCharge - maxCharge/_currentCharge));
            }
        }

        public string GetInteractText()
        {
            return String.Format("Turn {0}.", _lightToggle.enabled ? "off" : "on");
        }

        public void Interact()
        {
            Debug.Log("Toggling light");
            _lightToggle.enabled = !_lightToggle.enabled;
        }
        
    }
}