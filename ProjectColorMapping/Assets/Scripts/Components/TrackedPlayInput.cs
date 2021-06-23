using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using System;
using Components.Utils;

namespace Components
{
    public class TrackedPlayInput : MonoBehaviour
    {
        public GameCmdFactory cmdGameFactory;
        public TrackImageManager trackImageManager;

        private GameObject[] _trackers;

        void Start()
        {
            trackImageManager.OnTrackedImageChange
                .Subscribe(OnTrackedImageChange)
                .AddTo(this);
        }

        private void OnTrackedImageChange(GameObject[] obj)
        {
            Debug.Log("Tracking objects");
            _trackers = obj;
        }

        public void PlayClick()
        {
            float[] srcValue = AIRHandler.CalculateMarkerImageVertex(_trackers[0]);
            Texture2D screenShotTex = Screenshot.GetScreenShot(_trackers[1]);
            
            cmdGameFactory.PlayTurnInput(trackImageManager, srcValue, screenShotTex).Execute();
        }
    }
}
