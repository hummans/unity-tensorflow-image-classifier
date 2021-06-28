using System;
using UniRx;
using UnityEngine;
using ViewModel;

namespace Infrastructure
{
    public class PlayTurnGateway : IApi
    {
        public string URL_DATA { get; set; }

        public IObservable<Unit> PlayTurn(TrackImageManager srcValue, Texture2D textureScreenshoot)
        {
            /*AIRHandler.ProcessColoredMapTexture(textureScreenshoot, srcValue, trackerManager.realWidth, trackerManager.realHeight, (resultTex) =>
            {
                drawObj = GameObject.FindGameObjectWithTag("coloring");
                drawObj.GetComponent<Renderer>().material.mainTexture = resultTex;
            });*/
            return Observable.Return(Unit.Default)
                .Delay(TimeSpan.FromMilliseconds(500));
        }
    }
}