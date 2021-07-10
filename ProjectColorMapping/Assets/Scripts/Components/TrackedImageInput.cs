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
        public GameCmdFactory cmdGameFactory;
        public TrackImageManager trackImageManager;
        public DebugConsole debugConsole;

        [Header("UI")]
        public Button trackInput;
        public TextMeshProUGUI trackLabel;
        public RawImage imageScreenshoot;
        
        private GameObject[] _trackers;

        void Start()
        {
            debugConsole.logInput.Value = "Creating Runtime Mutable Image Library";
            
            var lib = trackedImageManager.CreateRuntimeLibrary(_runtimeImageLibrary);
            trackedImageManager.referenceLibrary = lib;
            trackedImageManager.maxNumberOfMovingImages = _maxNumberOfImages;
            trackedImageManager.enabled = true;

            trackImageManager.OnTrackedImageChange
                .Subscribe(OnTrackedImageChange)
                .AddTo(this);

            trackInput.onClick.AddListener(() => StartCoroutine(PlayCapture()));
        }

        private void OnTrackedImageChange(GameObject[] _trackers)
        {
            this._trackers = _trackers;
        }
   
        private IEnumerator PlayCapture()
        {
            yield return new WaitForEndOfFrame();
            trackLabel.text = "[Play] Capturing Image..";

            Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera,debugConsole);
            
            imageScreenshoot.gameObject.SetActive(true);
            imageScreenshoot.texture = screenShotTex;

            StartCoroutine(AddImageJob(screenShotTex, screenShotTex.width, screenShotTex.height, 0.3f));
        }     
        public IEnumerator AddImageJob(Texture2D texture2D, int widthInPixels, int heightInPixels, float widthInMeters)
        {
            yield return null;
    
            debugConsole.logInput.Value = "Adding image";

            trackLabel.text = "[Play] Job Starting...";

            NativeArray<byte> data = texture2D.GetRawTextureData<byte>();

            var aspectRatio = (float)widthInPixels / (float)heightInPixels;
            var sizeInMeters = new Vector2(widthInMeters, widthInMeters * aspectRatio);

            var referenceImage = new XRReferenceImage(
                // Guid is assigned after image is added
                SerializableGuid.empty,
                // No texture associated with this reference image
                SerializableGuid.empty,
                sizeInMeters, "My Image", null);

            try
            {
                if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
                {
                    // use the mutableLibrary
                    MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = trackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
                             
                    var jobState = mutableLibrary.ScheduleAddImageWithValidationJob(texture2D, Guid.NewGuid().ToString(), 0.1f);

                    while(!jobState.jobHandle.IsCompleted)
                    {
                        trackLabel.text = "[Play] Job Running...";
                    }

                    trackInput.enabled = false;
                    trackLabel.text = "[Play] Job Completed...";
                    
                    debugConsole.logInput.Value = $"Job Completed ({mutableRuntimeReferenceImageLibrary.count})";
                    debugConsole.logInput.Value = $"Supported Texture Count ({mutableRuntimeReferenceImageLibrary.supportedTextureFormatCount})";
                    debugConsole.logInput.Value = $"trackImageManager.trackables.count ({trackedImageManager.trackables.count})";
                    debugConsole.logInput.Value = $"trackImageManager.maxNumberOfMovingImages ({trackedImageManager.maxNumberOfMovingImages})";
                    debugConsole.logInput.Value = $"trackImageManager.supportsMutableLibrary ({trackedImageManager.subsystem.subsystemDescriptor.supportsMutableLibrary})";
                    debugConsole.logInput.Value = $"trackImageManager.requiresPhysicalImageDimensions ({trackedImageManager.subsystem.subsystemDescriptor.requiresPhysicalImageDimensions})";

                    Invoke("ReEnableInput", 5f);
                } 
                else 
                {
                    debugConsole.logInput.Value = "trackedImageManager.referenceLibrary is not Mutable";
                }          
            }
            catch(Exception e)
            {
                if(texture2D == null)
                {
                    debugConsole.logInput.Value = "texture2D is null";    
                }
                Debug.Log($"Error: {e.ToString()}");
            }
        }

        void ReEnableInput()
        {
            trackInput.enabled = true;
            trackLabel.text = "Play Capture";
        }

        private void CaptureColors(Texture2D textureCaptured)
        {     
            trackInput.enabled = false;
            debugConsole.logInput.Value = $"Go in trackedImageInput.Play (Procces Gateway)";
            trackLabel.text = "[RGB] Capturing colors..";
            string[] scrValues = new string[1];

            cmdGameFactory.PlayTurnInput(trackImageManager, textureCaptured, scrValues);
        }
    }
}
