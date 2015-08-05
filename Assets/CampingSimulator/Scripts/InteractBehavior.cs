using System;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    public interface IInteractable
    {
        void Interact();
    }

    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class InteractBehavior : MonoBehaviour
    {
        void Start()
        {
            
        }

        void Update()
        {


        }
        
    }
}