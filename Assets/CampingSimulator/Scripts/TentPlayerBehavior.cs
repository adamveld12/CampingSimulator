using System;
using UnityEngine;

namespace Assets.CampingSimulator.Scripts
{
    [Serializable]
    public class TentPlayerBehavior : MonoBehaviour
    {
        private Camera _fpCamera;

        [SerializeField] 
        MouseLook MouseLook;

        private FlashlightBehavior _flashlight;

        private bool oldState, newState;

        // Use this for initialization
        void Start()
        {
            _fpCamera = GetComponentInChildren<Camera>();
            _flashlight = GetComponentInChildren<FlashlightBehavior>();
            MouseLook.Init(transform, _fpCamera.transform);
        }

        // Update is called once per frame
        void Update()
        {
            oldState = newState; 
            newState = Input.GetAxis("ToggleLight") > 0;
            
            if (!newState && oldState)
                _flashlight.Interact();

            MouseLook.LookRotation(transform, _fpCamera.transform);
//            CheckForInteractable();
        }

//        private void CheckForInteractable()
//        {
//            Vector3 fwd = _fpCamera.transform.TransformDirection(Vector3.forward);
//            RaycastHit hitInfo;
//            if (Physics.Raycast(transform.position, fwd, out hitInfo, InteractionDistance))
//            {
//                var interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();
//                Debug.DrawLine(transform.position, hitInfo.point);
//                if (interactable != null)
//                {
//                    if (Input.GetMouseButtonDown(0))
//                        interactable.Interact();
//                }
//            }
//
//        }
    }
}