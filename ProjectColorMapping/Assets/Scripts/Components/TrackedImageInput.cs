using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ViewModel;
using Components.Utils;
using TMPro;
using System;

public class TrackedImageInput : MonoBehaviour
{
    [Header("Random")]
    public GameCmdFactory cmdGameFactory;
    public TrackImageManager trackImageManager;
    public DebugConsole debugConsole;
    public Camera arCamera;

    [Header("UI")]
    public Button trackInput;
    public TextMeshProUGUI trackLabel;
    public RawImage imageScreenshoot;
  

    private GameObject[] _trackers;
    
    void Start()
    {
        trackImageManager.OnTrackedImageChange
            .Subscribe(OnTrackedImageChange)
            .AddTo(this);

        trackInput.onClick.AddListener(OnClick);
    }
    private void OnTrackedImageChange(GameObject[] _trackers)
    {
        this._trackers = _trackers;
    }
    private void OnClick()
    {     
        trackInput.enabled = false;
        debugConsole.consoleLabel.Value = $"Go in trackedImageInput.Play (Procces Gateway)";
        trackLabel.text = "[RGB] Capturing colors..";

        cmdGameFactory.PlayTurnInput(trackImageManager, imageScreenshoot, _trackers, arCamera).Execute();
    }
}
