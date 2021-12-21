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
        public TrackManagerViewModel trackImageManager;
        public RawImage rawImage;

        private bool isFirst = true;

        void Start()
        {
            rawImage.color = new Color(255,255,255,0);
            trackImageManager.widthTexture = rawImage.texture.width;
            trackImageManager.heightTexture = rawImage.texture.height;

            trackImageManager.currentTrackScreenshoot
                .Subscribe(OnScreenshoot)
                .AddTo(this);
        }

        private void OnScreenshoot(Texture screenshoot)
        {
            if(isFirst)
            {
                isFirst = false;
                return;
            }
            rawImage.color = new Color(255,255,255,1);
            rawImage.texture = screenshoot;
            Invoke("TurnOff", 2f);
        }

        void TurnOff()
        {
            rawImage.color = new Color(255,255,255,0);
        }
    }
}
