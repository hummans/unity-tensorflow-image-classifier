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
    public TrackedImageCmd TrackedImageChange(TrackImageManager trackerManager, GameObject arPrefab, GameObject arTarget)
    {
        return new TrackedImageCmd(trackerManager, arPrefab, arTarget);
    }
    public PlayTurnCmd PlayTurnInput(TrackImageManager trackImageManager, Texture2D screenShotTex, string[] scrValues)
    {
        return new PlayTurnCmd(trackImageManager, screenShotTex, scrValues, new PlayTurnGateway());
    }
}