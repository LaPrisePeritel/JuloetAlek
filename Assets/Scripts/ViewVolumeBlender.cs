using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class ViewVolumeBlender : AViewVolume
    {
        public static ViewVolumeBlender Instance;

        private List<AViewVolume> activeViewVolumes;
        private Dictionary<AView, List<AViewVolume>> volumesPerViews;


        public void AddVolume(AViewVolume volume)
        {
            activeViewVolumes.Add(volume);
            if (!volumesPerViews.ContainsKey(View))
            {
                volumesPerViews[View] = new List<AViewVolume>();
                SetActive(true);
            }
            volumesPerViews[View].Add(volume);
        }

        private void OnGUI()
        {
            foreach (AViewVolume v in activeViewVolumes)
            {
                GUILayout.Label(v.name);
            }
        }

        public void RemoveVolume(AViewVolume volume)
        {
            activeViewVolumes.Remove(volume);
            volumesPerViews[View].Remove(volume);
            if (!volumesPerViews.ContainsKey(View))
            {
                volumesPerViews.Remove(View);
                SetActive(false);
            }
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null && Instance != this)
                Destroy(this);
        }
    }
}
