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
    private readonly Texture2D screenShotTex;
    private readonly string[] scrValues;
    private readonly RawImage rawImage;
    private GameObject[] trackers;
    private Camera arCamera;
    private PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(TrackImageManager trackImageManager, Texture2D screenShotTex, string[] scrValues, PlayTurnGateway playTurnGateway)
    {
        this.trackImageManager = trackImageManager;
        this.screenShotTex = screenShotTex;
        this.scrValues = scrValues;
        this.playTurnGateway = playTurnGateway;
    }

    public void Execute()
    {       
        playTurnGateway.PlayTurn(trackImageManager, screenShotTex)
            .Subscribe();
    } 
}