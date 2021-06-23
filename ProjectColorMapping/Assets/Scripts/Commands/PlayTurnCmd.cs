using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Infrastructure;

public class PlayTurnCmd : ICommand
{
    private TrackImageManager trackerManager;
    private float[] srcValue;
    private Texture2D textureScreenshoot;
    private readonly PlayTurnGateway playTurnGateway;

    public PlayTurnCmd(TrackImageManager trackerManager, float[] srcValue, Texture2D textureScreenshoot, PlayTurnGateway playTurnGateway)
    {
        this.trackerManager = trackerManager;
        this.srcValue = srcValue;
        this.textureScreenshoot = textureScreenshoot;
        this.playTurnGateway = playTurnGateway;
    }

    public void Execute()
    {
        // Play       
        playTurnGateway.PlayTurn(trackerManager, srcValue, textureScreenshoot)
            .Do(_ => Debug.Log("Find results"))
            .Subscribe();
    } 
}