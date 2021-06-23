using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageCmd : ICommand
{
    private TrackImageManager trackerManager;
    private GameObject arPrefab;
    private GameObject arTarget;

    public TrackedImageCmd(TrackImageManager trackerManager, GameObject arPrefab, GameObject arTarget)
    {
        this.trackerManager = trackerManager;
        this.arPrefab = arPrefab;
        this.arTarget = arTarget;
    }

    public void Execute()
    {
        GameObject[] trackChange = { arPrefab, arTarget };
        trackerManager.OnTrackedImageChange.OnNext(trackChange);
    } 
}