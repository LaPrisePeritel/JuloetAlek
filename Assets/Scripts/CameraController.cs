using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TPCamera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Camera Camera;
        public CameraConfiguration CameraConfigurator;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);

        }

        public void ApplyConfiguration(Camera camera, CameraConfiguration configuration)
        {
            Camera = camera;
            CameraConfigurator = configuration;
        }


    }
}

