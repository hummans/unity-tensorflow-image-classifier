using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Infrastructure;
using Components.Utils;
using TMPro;
using UnityEngine.UI;

public class PlayTurnCmd : ICommand
{
    private readonly TrackImageManager trackImageManager;
    private readonly RawImage rawImage;
    private GameObject[] trackers;
    private Camera arCamera;
    private PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(TrackImageManager trackImageManager, RawImage rawImage, GameObject[] trackers, Camera arCamera, PlayTurnGateway playTurnGateway)
    {
        this.trackImageManager = trackImageManager;
        this.rawImage = rawImage;
        this.trackers = trackers;
        this.arCamera = arCamera;
        this.playTurnGateway = playTurnGateway;
    }

    public void Execute()
    {   
        trackers[0].SetActive(false);

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);

        trackers[0].SetActive(true);
        rawImage.texture = screenShotTex;
        
        playTurnGateway.PlayTurn(trackImageManager, screenShotTex)
            .Subscribe();
    } 
}