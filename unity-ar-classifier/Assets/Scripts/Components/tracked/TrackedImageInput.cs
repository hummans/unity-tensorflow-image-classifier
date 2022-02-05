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
        public Camera arCamera;

        [Header("Data")]
        public GameCmdFactory gameCmdFactory;
        public TrackManagerViewModel trackData;


        void Awake()
        {
            
            if(!Application.isEditor)
            {
                Debug.Log("Creating Runtime Mutable Image Library");
                var lib = trackedImageManager.CreateRuntimeLibrary(trackData.runtimeImageLibrary);
                trackedImageManager.referenceLibrary = lib;
            }

            trackedImageManager.requestedMaxNumberOfMovingImages = trackData.maxNumberOfImages;
        }

        public void OnClick()
        {
            gameCmdFactory.PlayTurnInput(trackedImageManager, arCamera, trackData).Execute();
        }
    }
}

