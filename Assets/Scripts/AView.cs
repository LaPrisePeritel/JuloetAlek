using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public abstract class AView : MonoBehaviour
    {
        public float Weight;
        public bool IsActiveOnStart;
        
        public virtual CameraConfiguration GetConfiguration() => null;

        public void SetActive(bool isActive) 
        {
            CameraController.Instance.AddView(this);
        }

        private void Start()
        {
            if (IsActiveOnStart)
                SetActive(IsActiveOnStart);
        }
    }
}
