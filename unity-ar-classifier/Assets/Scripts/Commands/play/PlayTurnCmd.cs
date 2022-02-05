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
    private TrackManagerViewModel trackData;
    private PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(ARTrackedImageManager trackedImageManager, Camera arCamera, TrackManagerViewModel trackData, PlayTurnGateway playTurnGateway)
    {
        this.trackedImageManager = trackedImageManager;
        this.arCamera = arCamera;
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
        trackData.currentTrackInterfaceActive.Value = false;
        trackData.currentTrackLabel.Value = "[PLAY] Play Process..";

        Debug.Log($"-- Play input received! --");

        playTurnGateway.PlayTurn(trackData, textureCrop)
            // .Do(_ = >) Call to OnNext color
            .Do(_ => Debug.Log($"-- Process runtime library! --"))
            .Do(value => AddImageRuntimeLibrary(value, textureCrop, screenShotTex))
            .Subscribe();   
    }

    private void AddImageRuntimeLibrary(bool execute, Texture2D textureCrop, Texture2D screenShotTex)
    {
        if(!execute)
        {
            Debug.LogError("Play interrupted because has an HTTP Error!");
            return;
        }

        RuntimeLibraryManager.AddImageRuntimeLibrary(screenShotTex, trackedImageManager, trackData)
                        .Do(_ => ResetInput())
                        .Subscribe();
    }
    private List<Texture2D> GetTexture()
    {
        trackData.ARTrackedEnable.Value = false;
        trackData.currentTrackLabel.Value = "[Play] Capturing Image..";

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera);
        Texture2D textureCrop = Screenshot.CropScreenshoot(Screenshot.ResampleAndCrop(screenShotTex, trackData.widthTexture, trackData.heightTexture));
        trackData.currentTrackScreenshoot.Value = textureCrop;

        return new List<Texture2D>{
            textureCrop,
            screenShotTex
        };
    }

    private async void ResetInput()
    {
        trackData.ARTrackedEnable.Value = true;
        await Task.Delay(TimeSpan.FromSeconds(2));
        Debug.Log($"Reseting play input button!");

        trackData.currentTrackLabel.Value = "Play Capture";
        trackData.currentTrackInterfaceActive.Value = true;
    }
}