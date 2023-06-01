using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TPCamera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Camera Camera;

        private List<AView> activeViews = new List<AView>();

        public CameraConfiguration InitialConfiguration;
        public CameraConfiguration CurrentConfiguration;
        public CameraConfiguration TargetConfiguration;

        [SerializeField]
        private float CameraTransitionSpeedValue;
        [SerializeField]
        private float CameraTransitionSpeedCurrent;

        private float StartDistance;

        public enum InterpolationType
        {
            None,
            Linear,
            Smooth
        }

        public InterpolationType TypeInterpolation;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);

        }

        private void Start()
        {
            CurrentConfiguration = new(Camera.transform.rotation.eulerAngles.x, Camera.transform.rotation.eulerAngles.y, Camera.transform.rotation.eulerAngles.z, 0, Camera.fieldOfView, Camera.transform.position);
            InitialConfiguration = new(Camera.transform.rotation.eulerAngles.x, Camera.transform.rotation.eulerAngles.y, Camera.transform.rotation.eulerAngles.z, 0, Camera.fieldOfView, Camera.transform.position);
            SetTargetConfiguration();
        }

        private void Update()
        {
            switch (TypeInterpolation)
            {
                case InterpolationType.Linear:
                    break;
                case InterpolationType.Smooth:
                    SmoothInterpolation();
                    break;
            }
        }

        public void ChangeInterpolation(InterpolationType type)
        {
            CameraTransitionSpeedCurrent = CameraTransitionSpeedValue;
            CurrentConfiguration = new(InitialConfiguration);
            Camera.transform.SetPositionAndRotation(InitialConfiguration.GetPosition(), InitialConfiguration.GetRotation());
            SetTargetConfiguration();
            TypeInterpolation = type;
        }

        private void SmoothInterpolation()
        {
            if(CameraTransitionSpeedCurrent > 0.001f)
            {
                CameraTransitionSpeedCurrent = (Vector3.Distance(TargetConfiguration.GetPosition(), CurrentConfiguration.GetPosition()) / StartDistance) * CameraTransitionSpeedValue;
                float speedTransition = CameraTransitionSpeedValue * Time.deltaTime;
                CurrentConfiguration.Pivot += (TargetConfiguration.GetPosition() - CurrentConfiguration.GetPosition()) * speedTransition;
                Camera.transform.position = Vector3.Slerp(CurrentConfiguration.GetPosition(), TargetConfiguration.GetPosition(), speedTransition);
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, TargetConfiguration.GetRotation(), speedTransition);
                Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, TargetConfiguration.Fov, speedTransition);

                CurrentConfiguration.Yaw = Camera.transform.rotation.eulerAngles.x;
                CurrentConfiguration.Pitch = Camera.transform.rotation.eulerAngles.y;
                CurrentConfiguration.Roll = Camera.transform.rotation.eulerAngles.z;
            }
            else
            {
                CameraTransitionSpeedCurrent = 0;
                StopInterpolation();
            }
        }

        private void LinearInterlopation()
        {

        }
        private void StopInterpolation()
        {
            ApplyConfiguration(TargetConfiguration);
            TypeInterpolation = InterpolationType.None;
        }
        public void SetTargetConfiguration()
        {
            TargetConfiguration = ComputeAverageConfiguration();
            StartDistance = Vector3.Distance(TargetConfiguration.Pivot, CurrentConfiguration.Pivot);
        }

        public void ApplyConfiguration(CameraConfiguration config)
        {
            CurrentConfiguration = config;
            Camera.transform.rotation = config.GetRotation();
            Camera.transform.position = config.GetPosition();
            Camera.fieldOfView = config.Fov;

        }


        public void AddView(AView view) => activeViews.Add(view);

        public void RemoveView(AView view) => activeViews.Remove(view);

        public CameraConfiguration ComputeAverageConfiguration()
         => new(ComputeAverageYaw(), ComputeAveragePitch(), ComputeAverageRoll(), ComputeAverageDistance(), ComputeAverageFov(), ComputeAveragePivot());

        public float ComputeAverageYaw()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Yaw * Mathf.Deg2Rad), Mathf.Sin(configuration.Yaw * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }
        
        public float ComputeAveragePitch()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Pitch * Mathf.Deg2Rad), Mathf.Sin(configuration.Pitch * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }

        public float ComputeAverageRoll()
        {
            Vector2 sum = Vector2.zero;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(configuration.Roll * Mathf.Deg2Rad), Mathf.Sin(configuration.Roll * Mathf.Deg2Rad)) * view.Weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }

        public Vector3 ComputeAveragePivot()
        {
            Vector3 sum = Vector3.zero;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Pivot * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }
        
        public float ComputeAverageDistance()
        {
            float sum = 0f;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Distance * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }
        
        public float ComputeAverageFov()
        {
            float sum = 0f;
            float weightSum = 0f;
            foreach (AView view in activeViews)
            {
                CameraConfiguration configuration = view.GetConfiguration();
                sum += configuration.Fov * view.Weight;
                weightSum += view.Weight;
            }
            return (sum / weightSum);
        }

        void OnGUI()
        {

            if (GUI.Button(new Rect(10, 10, 200, 30), "Linear Interpolation"))
                ChangeInterpolation(InterpolationType.Linear);

            if (GUI.Button(new Rect(10, 70, 200, 30), "Smooth Interpolation"))
                ChangeInterpolation(InterpolationType.Smooth);
        }

        private void OnDrawGizmos()
        {
            CurrentConfiguration.DrawGizmos(Color.red);
            TargetConfiguration.DrawGizmos(Color.green);
        }
    }
}

