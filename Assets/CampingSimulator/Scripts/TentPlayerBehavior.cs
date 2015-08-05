using UnityEngine;

namespace Assets.CampingSimulator
{

    public class TentPlayerBehavior : MonoBehaviour
    {

        [SerializeField]
        private MouseLook MouseLook;
        [SerializeField]
        private Camera FPCamera;

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
        }
    }
}