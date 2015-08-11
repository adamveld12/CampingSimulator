using System.Collections;

namespace Assets.CampingSimulator.Scripts
{
    public interface IEncounter
    {
        IEnumerator Begin(IEncounterManager em);
        void Abort();
    }
}