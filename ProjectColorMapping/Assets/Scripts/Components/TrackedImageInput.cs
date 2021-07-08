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

public class TrackedImageInput : MonoBehaviour
{
    [Header("AR")]
    public ARTrackedImageManager trackedImageManager;
    [SerializeField] private XRReferenceImageLibrary _runtimeImageLibrary;
    public Camera arCamera;

    [Header("Data")]
    public GameCmdFactory cmdGameFactory;
    public TrackImageManager trackImageManager;
    public DebugConsole debugConsole;

    [Header("UI")]
    public Button trackInput;
    public TextMeshProUGUI trackLabel;
    public RawImage imageScreenshoot;
    
    private GameObject _prefabOnTrack;
    private GameObject[] _trackers;

    [Obsolete]
    void Awake()
    {
        debugConsole.logInput.Value = "Creating Runtime Mutable Image Library";
        
        var lib = trackedImageManager.CreateRuntimeLibrary(_runtimeImageLibrary);
        trackedImageManager.referenceLibrary = lib;
        trackedImageManager.maxNumberOfMovingImages = 1;
        trackedImageManager.trackedImagePrefab = _prefabOnTrack;

        trackImageManager.OnTrackedImageChange
            .Subscribe(OnTrackedImageChange)
            .AddTo(this);

        trackInput.onClick.AddListener(() => StartCoroutine(PlayCapture()));
    }

    private void OnTrackedImageChange(GameObject[] _trackers)
    {
        this._trackers = _trackers;
    }

    [Obsolete]
    private IEnumerator PlayCapture()
    {
        yield return new WaitForEndOfFrame();
        trackLabel.text = "[Play] Capturing Image..";

        //_trackers[0].SetActive(false);

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera,debugConsole);
        
        imageScreenshoot.gameObject.SetActive(true);
        imageScreenshoot.texture = screenShotTex;

        //_trackers[0].SetActive(true);

        StartCoroutine(AddImageJob(screenShotTex));
    }

    [Obsolete]
    public IEnumerator AddImageJob(Texture2D texture2D)
    {
        yield return null;
   
        debugConsole.logInput.Value = "Adding image";

        trackLabel.text = "[Play] Job Starting...";

        var firstGuid = new SerializableGuid(0,0);
        var secondGuid = new SerializableGuid(0,0);
        
        XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f,0.1f), Guid.NewGuid().ToString(), texture2D);

        try
        {
            if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                // use the mutableLibrary
                MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = trackedImageManager.CreateRuntimeLibrary(_runtimeImageLibrary) as MutableRuntimeReferenceImageLibrary;
                
                debugConsole.logInput.Value = $"TextureFormat.RGBA32 supported: {mutableRuntimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}";
                debugConsole.logInput.Value = $"TextureFormat size: {texture2D.width}px width {texture2D.height}px height";
                
                var jobHandle = mutableRuntimeReferenceImageLibrary.ScheduleAddImageJob(texture2D, Guid.NewGuid().ToString(), 0.1f);

                while(!jobHandle.IsCompleted)
                {
                    trackLabel.text = "[Play] Job Running...";
                }

                trackLabel.text = "[Play] Job Completed...";
                debugConsole.logInput.Value = "AddImageJob completed";
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

    private void CaptureColors(Texture2D textureCaptured)
    {     
        trackInput.enabled = false;
        debugConsole.logInput.Value = $"Go in trackedImageInput.Play (Procces Gateway)";
        trackLabel.text = "[RGB] Capturing colors..";
        string[] scrValues = new string[1];

        cmdGameFactory.PlayTurnInput(trackImageManager, textureCaptured, scrValues);
    }
}
