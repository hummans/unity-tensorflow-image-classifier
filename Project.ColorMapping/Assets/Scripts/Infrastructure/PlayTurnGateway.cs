using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using ViewModel;
using Random=UnityEngine.Random;


namespace Infrastructure
{
    public class PlayTurnGateway : IPlayGateway
    {
        public string URL_DATA { get; set; }

        public IObservable<Unit> PlayTurn(TrackManager trackImageManagerData, Texture2D textureScreenshoot)
        {
            return Observable.FromCoroutine<Unit>(observer => GetImageColor(observer, trackImageManagerData, textureScreenshoot));
        }

        IEnumerator GetImageColor(IObserver<Unit> observer, TrackManager trackImageManagerData, Texture2D textureScreenshoot)
        {
            yield return new WaitForSeconds(3);

            ColorObject colorObject = GenerateColor(5);

            trackImageManagerData.ColorObject.Value = colorObject;
            Debug.Log($"[RGB] One is {colorObject.Colors[0]}");

            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
        
        private ColorObject GenerateColor(int count)
        {
            List<Color> colors = new List<Color>();

            for (int i = 0; i < count; i++)
            {
                byte r = Convert.ToByte(Random.Range(0,255));
                byte g = Convert.ToByte(Random.Range(0,255));
                byte b = Convert.ToByte(Random.Range(0,255));

                colors.Add(new Color32(r, g, b, 255));    
            }

            return new ColorObject
            {
                Colors = colors
            };
        }
    }
}