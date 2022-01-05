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
    public PlayTurnCmd PlayTurnInput(ARTrackedImageManager trackedImageManager, Camera arCamera, 
    TrackManagerViewModel trackImageManagerData)
    {
        return new PlayTurnCmd(trackedImageManager, arCamera, trackImageManagerData, new PlayTurnGateway());
    }
}