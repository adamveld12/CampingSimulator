using System;
using System.Collections;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    public abstract class Encounter
    {
        [SerializeField]
        public  string _name;
        private readonly Guid _id = Guid.NewGuid();
        private readonly IEncounterManager _encounterManager;
        private bool _aborted;

        protected Encounter(IEncounterManager encounterManager, string name)
        {
            Debug.Assert(encounterManager != null, "encounter manager is null");
            _encounterManager = encounterManager;
            _name = name;
        }

        protected abstract void InitEncounter();
        protected abstract IEnumerator DoEncounterAsync();

        protected abstract void BeginAbort();
        protected abstract void CompletedSuccessfully();

        protected virtual bool EarlyAbort()
        {
            return EncounterManager.IsPlayerLightOn();
        }

        public IEnumerator Start()
        {
            Debug.Log(string.Format("Initializing {0} encounter.", _name));
            yield return new WaitForSeconds(3);
            InitEncounter();

            Debug.Log(string.Format("Starting {0}-{1}.", _name, _id));
            var enumerator = DoEncounterAsync();

            while (enumerator.MoveNext())
            {
                if (!_aborted && !EarlyAbort())
                {
                    var encounterStep = enumerator.Current;
                    Debug.LogFormat("\trunning step {0}", encounterStep.GetType().Name);
                    yield return encounterStep;
                }
                else
                {
                    Abort();
                    break;
                }
            }

            yield return new WaitForSeconds(2);

            if(_encounterManager.IsPlayerLightOn())
                Abort();

            End();
        }

        private void End()
        {
            Debug.Log(string.Format("Completing {0}.", _name));

            if (!_aborted)
                CompletedSuccessfully();

            _encounterManager.Completed();
        }

        public void Abort()
        {
            if (_aborted) return;

            Debug.Log(string.Format("Aborting {0} encounter.", _name));
            _aborted = true;

            BeginAbort();
        }

        public Guid Id { get { return _id; } }
        public string Name { get { return _name; } }
        protected IEncounterManager EncounterManager { get { return _encounterManager; } }
    }
}