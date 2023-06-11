using UnityEngine;

namespace TPCamera
{
    public class TriggeredViewVolume : AViewVolume
    {
        private GameObject target;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == target)
                SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == target)
                SetActive(false);
        }
    }
}
