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
    private DebugConsole debugConsole;
    private TrackManager trackData;
    private PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(ARTrackedImageManager trackedImageManager, Camera arCamera, DebugConsole debugConsole, TrackManager trackImageManager, PlayTurnGateway playTurnGateway)
    {
        this.trackedImageManager = trackedImageManager;
        this.arCamera = arCamera;
        this.debugConsole = debugConsole;
        this.trackData = trackImageManager;
        this.playTurnGateway = playTurnGateway;
    }

    public void Execute()
    {       
        List<Texture2D> textures = GetTexture();
        CaptureTypeProcess(textures[0], textures[1]);
    }

    private List<Texture2D> GetTexture()
    {
        trackData.ARTrackedEnable.Value = false;
        trackData.currentTrackLabel.Value = "[Play] Capturing Image..";

        Texture2D screenShotTex = Screenshot.GetScreenShot(arCamera, debugConsole);
        Texture2D textureCrop = Screenshot.Save(Screenshot.ResampleAndCrop(screenShotTex, trackData.widthTexture, trackData.heightTexture));
        trackData.currentScreenshoot.Value = screenShotTex;

        return new List<Texture2D>{
            textureCrop,
            screenShotTex
        };
    }

    private IObservable<Unit> AddImageRuntimeLibrary(Texture2D textureCrop, Texture2D screenShotTex)
    {
        return Observable.FromCoroutine<Unit>((observer, cancellationToke) => AddImageJob(textureCrop, screenShotTex, 0.3f, observer, cancellationToke));
    }  
           
    public IEnumerator AddImageJob(Texture2D textureCrop, Texture2D texture2D, float widthInMeters, 
    IObserver<Unit> observer, CancellationToken cancellationToken)
    {
        yield return null;

        debugConsole.logInput.Value = "Adding image";
        trackData.currentTrackLabel.Value = "[Play] Job Starting...";
        
        NativeArray<byte> data = texture2D.GetRawTextureData<byte>();
        var aspectRatio = (float) texture2D.width / (float)texture2D.height;
        var sizeInMeters = new Vector2(widthInMeters, widthInMeters * aspectRatio);
        var referenceImage = new XRReferenceImage(
            // Guid is assigned after image is added
            SerializableGuid.empty,
            // No texture associated with this reference image
            SerializableGuid.empty,
            sizeInMeters, "My Image", null);

        try
        {
            // Set screenshoot to tracked
            if (trackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                // Use the mutableLibrary
                MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = trackedImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
                         
                var jobState = mutableLibrary.ScheduleAddImageWithValidationJob(
                    texture2D, 
                    Guid.NewGuid().ToString(), // Name
                    0.5f);
                
                while(!jobState.jobHandle.IsCompleted)
                {
                    trackData.currentTrackLabel.Value = "[Play] Job Running...";
                }

                trackData.currentTrackActive.Value = false;
                trackData.currentTrackLabel.Value = "[Play] Job Completed...";
                
                debugConsole.logInput.Value = $"Job Completed ({mutableRuntimeReferenceImageLibrary.count})";
                debugConsole.logInput.Value = $"Supported Texture Count ({mutableRuntimeReferenceImageLibrary.supportedTextureFormatCount})";
                debugConsole.logInput.Value = $"trackImageManager.trackables.count ({trackedImageManager.trackables.count})";
                debugConsole.logInput.Value = $"trackImageManager.supportsMutableLibrary ({trackedImageManager.subsystem.subsystemDescriptor.supportsMutableLibrary})";
                debugConsole.logInput.Value = $"trackImageManager.requiresPhysicalImageDimensions ({trackedImageManager.subsystem.subsystemDescriptor.requiresPhysicalImageDimensions})";
            } 
            else 
            {
                debugConsole.logInput.Value = "trackedImageManager.referenceLibrary is not Mutable";
            }          
            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }
        catch(Exception e)
        {
            if(texture2D == null)
            {
                debugConsole.logInput.Value = "Texture2D is null";    
                debugConsole.logInput.Value = $"Error: {e.ToString()}";    
            }
            observer.OnError(e);
            Debug.Log($"Error: {e.ToString()}");
        }
    }

    private void CaptureTypeProcess(Texture2D textureCrop, Texture2D screenShotTex)
    {
        // Find colors in the image   
        trackData.currentTrackActive.Value = false;
        debugConsole.logInput.Value = $"Play input received!";
        debugConsole.logInput.Value = $"We are in process to connect server in tracked gateway!";
        trackData.currentTrackLabel.Value = "[RGB] Capturing colors..";

        playTurnGateway.PlayTurn(trackData, textureCrop)
            .Do(_ => 
                    AddImageRuntimeLibrary(textureCrop, screenShotTex)
                        .Do(_ => trackData.ARTrackedEnable.Value = true)
                        .Do(_ => ResetInput())
                        .Subscribe()
            )
            .Subscribe();
    }

    private async void ResetInput()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        debugConsole.logInput.Value = $"Reseting play input button!";

        trackData.currentTrackLabel.Value = "Play Capture";
        trackData.currentTrackActive.Value = true;
    }
}