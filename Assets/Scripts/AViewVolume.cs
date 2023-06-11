using UnityEngine;

namespace TPCamera
{
    public abstract class AViewVolume : MonoBehaviour
    {
        public int Priority = 0;
        public AView View;

        private int Uid;
        static int NextUid = 0;

        protected bool IsActive { get; private set; }

        public virtual float ComputeSelfWeight() => 1.0f;

        private void Awake()
        {
            Uid = NextUid;
            NextUid++;
        }

        protected void SetActive(bool isActive)
        {
            IsActive = isActive;
            if (IsActive)
                ViewVolumeBlender.Instance.AddVolume(this);
            else
                ViewVolumeBlender.Instance.RemoveVolume(this);

        }
    }
}
