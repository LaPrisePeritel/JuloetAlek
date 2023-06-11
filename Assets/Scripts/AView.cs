using UnityEngine;

namespace TPCamera
{
    public abstract class AView : MonoBehaviour
    {
        public float Weight;
        
        public virtual CameraConfiguration GetConfiguration() => null;

        public void SetActive(bool isActive) 
        {
            CameraController.Instance.AddView(this);
        }
    }
}
