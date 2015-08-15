using System;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    [RequireComponent(typeof(Animation))]
    public class SimpleEncounter : MonoBehaviour, IEncounter
    {
        private Animation _animation;
        private AudioSource _source;
        private IEncounterManager _em;

        // Use this for initialization
        void Start()
        {
        }

        public void StartFootsteps()
        {
            Debug.Log("Starting footsteps");
            _source.Play();
        }

        public void Begin()
        {
            _em = GetComponentInParent<IEncounterManager>();
            _animation = GetComponent<Animation>();
            _source = GetComponentInChildren<AudioSource>();

            Debug.Assert(_em != null, "encounter manager is null");
            Debug.Assert(_animation != null, "animation is null");
            Debug.Assert(_source != null, "source is null");

            var encounterImpl = new SimpleEncounterImplementation(_em, _animation, _source);
            StartCoroutine(encounterImpl.Start());
        }
    }
}
