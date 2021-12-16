
using System;
using System.Collections;
using System.Threading;
using UniRx;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ViewModel;

public static class RuntimeLibraryManager
{
    public static IObservable<Unit> AddImageRuntimeLibrary(Texture2D textureCrop, Texture2D screenShotTex, ARTrackedImageManager trackedImageManager
    , ConsoleViewModel debugConsole, TrackManagerViewModel trackData)
    {
        return Observable.FromCoroutine<Unit>((observer, cancellationToken) 
            => AddImageJob(textureCrop, screenShotTex, 0.3f, observer, cancellationToken, trackedImageManager, debugConsole, trackData));
    }  
           
    private static IEnumerator AddImageJob(Texture2D textureCrop, Texture2D texture2D, float widthInMeters, 
    IObserver<Unit> observer, CancellationToken cancellationToken, ARTrackedImageManager trackedImageManager
    ,ConsoleViewModel debugConsole, TrackManagerViewModel trackData)
    {
        yield return null;

        Debug.Log("Adding image");
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
                
                Debug.Log($"Job Completed ({mutableRuntimeReferenceImageLibrary.count})");
                Debug.Log($"Supported Texture Count ({mutableRuntimeReferenceImageLibrary.supportedTextureFormatCount})");
                Debug.Log($"trackImageManager.trackables.count ({trackedImageManager.trackables.count})");
                Debug.Log($"trackImageManager.supportsMutableLibrary ({trackedImageManager.subsystem.subsystemDescriptor.supportsMutableLibrary})");
                Debug.Log($"trackImageManager.requiresPhysicalImageDimensions ({trackedImageManager.subsystem.subsystemDescriptor.requiresPhysicalImageDimensions})");
            } 
            else 
            {
                Debug.Log("trackedImageManager.referenceLibrary is not Mutable");
            }          
            observer.OnNext(Unit.Default);
            observer.OnCompleted();
        }
        catch(Exception e)
        {
            if(texture2D == null)
            {
                Debug.LogError("Texture2D is null");    
            }
            observer.OnError(e);
            Debug.LogError($"Error: {e.ToString()}");
        }
    }
}