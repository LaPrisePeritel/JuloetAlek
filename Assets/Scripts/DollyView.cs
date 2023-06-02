using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
