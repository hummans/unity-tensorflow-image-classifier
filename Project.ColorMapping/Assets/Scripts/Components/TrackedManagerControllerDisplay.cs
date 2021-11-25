using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ViewModel;
using Components.Utils;
using TMPro;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace Components
{
    public class TrackedManagerControllerDisplay : MonoBehaviour
    {
        [Header("AR")]
        public ARTrackedImageManager trackedImageManager;
        public TrackManager trackData;

        void Start()
        {            
            trackData.ARTrackedEnable
                .Subscribe(OnEnableTrack)
                .AddTo(this);
            
            trackData.ARTrackedEnable.Value = false;
        }

        private void OnEnableTrack(bool isEnable)
        {
            trackedImageManager.enabled = isEnable;
        }
    }
}

