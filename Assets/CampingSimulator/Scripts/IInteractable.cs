using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    public class Encounter : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            
        }
    }

    public interface IInteractable
    {
        void Interact();
        string GetInteractText();
    }
}