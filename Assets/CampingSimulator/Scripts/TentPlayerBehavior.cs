using System;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    public class TentPlayerBehavior : MonoBehaviour
    {
        [SerializeField]
        private MouseLook MouseLook;

        [SerializeField]
        private Camera FPCamera;

        [SerializeField]
        private float InteractionDistance = 2;

        // Use this for initialization
        void Start()
        {
            FPCamera = Camera.main;
            MouseLook.Init(transform, FPCamera.transform);
        }

        // Update is called once per frame
        void Update()
        {
            MouseLook.LookRotation(transform, FPCamera.transform);
              CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            Vector3 fwd = FPCamera.transform.TransformDirection(Vector3.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, fwd, out hitInfo, InteractionDistance))
            {
                var interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
                Debug.DrawLine(transform.position, hitInfo.point);
                if (interactable != null)
                {
                    if (Input.GetMouseButtonDown(0))
                        interactable.Interact();
                }
            }

        }
    }
}