using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPCamera
{
    public class ViewVolumeBlender : AViewVolume
    {
        public static ViewVolumeBlender Instance;

        private List<AViewVolume> ActiveViewVolumes;
        private Dictionary<AView, List<AViewVolume>> VolumesPerViews;


        public void AddVolume(AViewVolume volume)
        {
            ActiveViewVolumes.Add(volume);
            if (!VolumesPerViews.ContainsKey(View))
            {
                VolumesPerViews[View] = new List<AViewVolume>();
                SetActive(true);
            }
            VolumesPerViews[View].Add(volume);
        }

        public void RemoveVolume(AViewVolume volume)
        {
            ActiveViewVolumes.Remove(volume);
            VolumesPerViews[View].Remove(volume);
            if (!VolumesPerViews.ContainsKey(View))
            {
                VolumesPerViews.Remove(View);
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
