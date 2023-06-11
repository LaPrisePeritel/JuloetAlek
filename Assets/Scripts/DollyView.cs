using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TPCamera
{
    public class DollyView : AView
    {

        public Transform Target;

        public float Distance;

        [Range(-180, 180)]
        public float Roll;

        [Range(0, 179)]
        public float Fov;

        public Rail MyRail;
        public float DistanceOnRail;
        public float Speed;

        public bool isAuto = false;

        protected override void Start()
        {
            base.Start();
            Target = MyRail.Bunny.transform;
        }
        public override CameraConfiguration GetConfiguration()
        {
            Vector3 direction = (Target.position - Camera.main.transform.position).normalized;

            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
            return new(yaw, pitch, Roll, 0, Fov, Target.transform.position);
        }
    }
}
