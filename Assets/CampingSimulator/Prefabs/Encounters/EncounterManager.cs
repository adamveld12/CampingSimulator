using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    public class EncounterManager : MonoBehaviour, IEncounterManager
    {
        private TentPlayerBehavior _tentPlayer;
        private FlashlightBehavior _tentPlayerLight;

        [SerializeField] 
        [Tooltip("The encounters the manager will choose from")] 
        public List<GameObject> Encounters;

        private IEncounter _current;

        // Use this for initialization
        void Start()
        {
            Debug.Assert(Encounters.Any(x => x.GetComponentInChildren<IEncounter>() == null), "There is an object that does not conform to the \"IEncounter\" interface in the EncounterManager");
            _tentPlayer = GetComponentInChildren<TentPlayerBehavior>();
            _tentPlayerLight = _tentPlayer.GetComponentInChildren<FlashlightBehavior>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsPlayerLightOn())
            {
                if (_current != null)
                    _current.Abort();
            }
            else
            {
                if (_current == null)
                {
                  var obj = (GameObject) Instantiate(Encounters.First(), Vector3.zero, Quaternion.identity);
                  _current = obj.GetComponent<IEncounter>();
                  StartCoroutine(_current.Begin(this));
                }

            }

            
        }

        public float PlayerChargeAmount()
        {
            return _tentPlayerLight.Charge;
        }

        public bool IsPlayerCharging()
        {
            return _tentPlayerLight.IsCharging;
        }

        public bool IsPlayerLightOn()
        {
            return _tentPlayerLight.IsOn;
        }

        public void Completed()
        {
            _current = null;
            Debug.Log("Completed Encounter");
        }
    }
}
