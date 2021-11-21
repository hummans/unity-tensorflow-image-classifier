using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;

namespace Components
{
    public class TrackedImageDisplay : MonoBehaviour
    {
        public TrackManager trackImageManager;
        public RawImage rawImage;

        void Start()
        {
            rawImage.color = new Color(255,255,255,0);
            trackImageManager.widthTexture = rawImage.texture.width;
            trackImageManager.heightTexture = rawImage.texture.height;
            
            Debug.Log($"Width of screenshoot is {trackImageManager.widthTexture}");
            Debug.Log($"Height of screenshoot is {trackImageManager.heightTexture}");

            trackImageManager.currentScreenshoot
                .Subscribe(OnScreenshoot)
                .AddTo(this);
        }

        private void OnScreenshoot(Texture screenshoot)
        {
            rawImage.color = new Color(255,255,255,1);
            rawImage.texture = screenshoot;
            Invoke("TurnOff", 3f);
        }

        void TurnOff()
        {
            rawImage.color = new Color(255,255,255,0);
        }
    }
}
