using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;
using System.Threading.Tasks;

namespace Components
{
    public class TrackedTakeScreenDisplay : MonoBehaviour
    {
        public TrackManagerViewModel trackImageManager;
        public RawImage rawImage;

        void Start()
        {
            trackImageManager.currentTrackInterfaceActive
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
