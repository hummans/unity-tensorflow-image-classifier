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
using System;
using System.Threading;
using Unity.Collections;
using Components;
using System.Threading.Tasks;

public class PlayTurnCmd : ICommand
{
    private ARTrackedImageManager trackedImageManager;
    private readonly Camera arCamera;
    private ViewModel.ConsoleViewModel debugConsole;
    private TrackManagerViewModel trackData;
    private PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(ARTrackedImageManager trackedImageManager, Camera arCamera, ViewModel.ConsoleViewModel debugConsole, TrackManagerViewModel trackData, PlayTurnGateway playTurnGateway)
    {
        this.trackedImageManager = trackedImageManager;
        this.arCamera = arCamera;
        this.debugConsole = debugConsole;
        this.trackData = trackData;
        this.playTurnGateway = playTurnGateway;
    }

    public void Execute()
    {       
        List<Texture2D> textures = GetTexture();
        PlayProcess(textures[0], textures[1]);
    }

    private void PlayProcess(Texture2D textureCrop, Texture2D screenShotTex)
    {
        // Find colors in the image   
        trackData.currentTrackActive.Value = false;
        trackData.currentTrackLabel.Value = "[PLAY] Play Process..";

        Debug.Log($"Play input received!");

        playTurnGateway.PlayTurn(trackData, textureCrop)
            // .Do(_ = >) Call to OnNext color
            .Do(_ => Debug.Log($"Process to add image to the runtime library"))
            .Do(_ => RuntimeLibraryManager.AddImageRuntimeLibrary(textureCrop, screenShotTex, trackedImageManager, debugConsole, trackData)
                        .Do(_ => trackData.ARTrackedEnable.Value = true)
                        .Do(_ => ResetInput())
                        .Subscribe())
            .Subscribe();   
    }

    private List<Texture2D> GetTexture()
    {
        trackData.ARTrackedEnable.Value = false;
        trackData.currentTrackLabel.Value = "[Play] Capturing Image..";

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera, debugConsole);
        Texture2D textureCrop = Screenshot.Save(Screenshot.ResampleAndCrop(screenShotTex, trackData.widthTexture, trackData.heightTexture));
        trackData.currentTrackScreenshoot.Value = screenShotTex;

        return new List<Texture2D>{
            textureCrop,
            screenShotTex
        };
    }

    private async void ResetInput()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        Debug.Log($"Reseting play input button!");

        trackData.currentTrackLabel.Value = "Play Capture";
        trackData.currentTrackActive.Value = true;
    }
}