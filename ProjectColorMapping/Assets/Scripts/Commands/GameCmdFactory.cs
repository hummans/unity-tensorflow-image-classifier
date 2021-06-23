using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ViewModel;

[CreateAssetMenu(fileName = "New Game Command Factory", menuName = "Command Factory/Game")]
public class GameCmdFactory : ScriptableObject
{
    public TrackedImageCmd TrackedImageChange(TrackImageManager trackerManager, GameObject arPrefab, GameObject arTarget)
    {
        return new TrackedImageCmd(trackerManager, arPrefab, arTarget);
    }
    public PlayTurnCmd PlayTurnInput(TrackImageManager trackerManager, float[] srcValue, Texture2D textureScreenshoot)
    {
        return new PlayTurnCmd(trackerManager, srcValue, textureScreenshoot, new PlayTurnGateway());
    }
}