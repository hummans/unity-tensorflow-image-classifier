using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ViewModel;

[CreateAssetMenu(fileName = "New Game Command Factory", menuName = "Command Factory/Game")]
public class GameCmdFactory : ScriptableObject
{
    public TrackedImageCmd TrackedImageChange(TrackManager trackerManager, GameObject arPrefab, GameObject arTarget)
    {
        return new TrackedImageCmd(trackerManager, arPrefab, arTarget);
    }
    public PlayTurnCmd PlayTurnInput(ARTrackedImageManager trackedImageManager, Camera arCamera, 
                                        DebugConsole debugConsole, TrackManager trackImageManagerData)
    {
        return new PlayTurnCmd(trackedImageManager, arCamera, debugConsole, trackImageManagerData, new PlayTurnGateway());
    }
}