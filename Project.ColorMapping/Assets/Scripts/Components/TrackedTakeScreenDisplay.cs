using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;

namespace Components
{
    public class TrackedTakeScreenDisplay : MonoBehaviour
    {
        public TrackManager trackImageManager;
        public RawImage rawImage;

        void Start()
        {
            trackImageManager.currentTrackActive
                .Subscribe(OnActive)
                .AddTo(this);
        }

        private void OnActive(bool isActive)
        {
            int transparency = isActive ? 1 : 0;
            rawImage.color = new Color(255,255,255,transparency);
        }
    }
}
