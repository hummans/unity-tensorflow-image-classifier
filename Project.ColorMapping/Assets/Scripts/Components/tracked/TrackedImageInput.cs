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
    public class TrackedImageInput : MonoBehaviour
    {
        [Header("AR")]
        public ARTrackedImageManager trackedImageManager;
        public XRReferenceImageLibrary _runtimeImageLibrary;
        public int _maxNumberOfImages;
        public Camera arCamera;

        [Header("Data")]
        public GameCmdFactory gameCmdFactory;
        public TrackManagerViewModel trackData;
        public ViewModel.ConsoleViewModel debugConsole;

        private GameObject[] _trackers;

        void Awake()
        {
            Debug.Log("Creating Runtime Mutable Image Library");
            
            var lib = trackedImageManager.CreateRuntimeLibrary(_runtimeImageLibrary);
            trackedImageManager.referenceLibrary = lib;
            trackedImageManager.requestedMaxNumberOfMovingImages = _maxNumberOfImages;

            trackData.OnTrackedImageChange
                .Subscribe(OnTrackedImageChange)
                .AddTo(this);
        }

        public void OnClick()
        {
            gameCmdFactory.PlayTurnInput(trackedImageManager, arCamera, debugConsole, trackData).Execute();
        }
        
        private void OnTrackedImageChange(GameObject[] _trackers)
        {
            this._trackers = _trackers;
        }
    }
}

