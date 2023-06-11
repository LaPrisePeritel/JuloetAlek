using UnityEngine;

namespace TPCamera
{
    public class TriggeredViewVolume : AViewVolume
    {
        public GameObject Target;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Target)
                SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Target)
                SetActive(false);
        }
    }
}
