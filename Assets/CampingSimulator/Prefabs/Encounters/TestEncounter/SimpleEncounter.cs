using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    [RequireComponent(typeof(Animation))]
    public class SimpleEncounter : MonoBehaviour, IEncounter
    {
        private Animation Timeline;

        private bool _done;
        private AudioSource[] _sources;
        private IEncounterManager _em;

        // Use this for initialization
        void Start()
        {
            Timeline = GetComponent<Animation>();
            _sources = GetComponentsInChildren<AudioSource>().OrderBy(x => x.gameObject.name).ToArray();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator Begin(IEncounterManager em)
        {
            _em = em;
                yield return new WaitForSeconds(3);

            Debug.Log("I see your light is off now..");
            Timeline.Play();

            while (Timeline.isPlaying)
            {
                if (_done)
                {
                  Destroy(gameObject);
                  yield break;
                }

                yield return new WaitForEndOfFrame();
            }


            Debug.Log("Boo");
            _done = true;
            em.Completed();
            Destroy(gameObject);
        }

        public void Abort()
        {
            Debug.Log("I got scared away");
            Timeline.Stop();
            _done = true;
            _em.Completed();
        }

        public void Play1()
        {
            Debug.Log("Playing 1");
            _sources[0].Play();
        }
        public void Play2()
        {
            Debug.Log("Playing 2");
            _sources[1].Play();
        }

        public void Play3()
        {
            Debug.Log("Playing 3");
            _sources[2].Play();
        }

    }
}
