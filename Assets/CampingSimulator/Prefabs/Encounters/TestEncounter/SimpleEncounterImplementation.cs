using System.Collections;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    public class SimpleEncounterImplementation : Encounter
    {
        private readonly Animation _animator;
        private readonly AudioSource _audioSource;

        public SimpleEncounterImplementation(IEncounterManager encounterManager, Animation animator, AudioSource audioSource) : base(encounterManager, "Simple Encounter")
        {
            _animator = animator;
            _audioSource = audioSource;
        }

        protected override void InitEncounter()
        {
            _animator.Play();
        }

        protected override IEnumerator DoEncounterAsync()
        {
            Debug.Log("I see your light is off now...");
            yield return new WaitForEndOfFrame();

            while (_animator.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }

            _audioSource.Stop();

        }

        protected override void CompletedSuccessfully()
        {
            Debug.Log("Boo");
        }


        protected override void BeginAbort()
        {
            _animator.Stop();
            _audioSource.Stop();
        }
    }
}