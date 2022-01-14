using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using Random=UnityEngine.Random;
using SimpleJSON;

namespace Infrastructure
{
    public class ColorTurnGateway : IColorGateway
    {
        private protected string URL_DATA = "https://ua6hsarvmg.execute-api.us-east-2.amazonaws.com/post_url_colormapping";
        
        public IObservable<Unit> ColorTurn(TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            return Observable.FromCoroutine<Unit>(observer => GetImageColor(observer, trackData, textureScreenshoot));
        }

        IEnumerator GetImageColor(IObserver<Unit> observer, TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            // Collect color
            string id = Random.Range(0,1000000).ToString();                
            string datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string postUrl = URL_DATA + $"?expiresin=120&key={id}_unity_colormapping_{datetime}.png";

            UnityWebRequest wwwGetPostUrl = UnityWebRequest.Get(postUrl);

            yield return wwwGetPostUrl.SendWebRequest();

            trackData.currentTrackLabel.Value = "[RGB] Collecting color..";        
         
            //RecognitionResponse colorObject = GenerateType(1);

            //trackData.currentRecognition.Value = colorObject;
            //Debug.Log($"[RGB] One is {colorObject.rgbColors[0]}");
        
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }

        /*
        private RecognitionResponse GenerateType(int count)
        {
            List<Color> colors = new List<Color>();

            for (int i = 0; i < count; i++)
            {
                byte r = Convert.ToByte(Random.Range(0,256));
                byte g = Convert.ToByte(Random.Range(0,256));
                byte b = Convert.ToByte(Random.Range(0,256));

                colors.Add(new Color32(r, g, b, 255));    
            }

            return new RecognitionResponse
            {
                idModel = Random.Range(1,3),
                rgbColors = colors
            };
        }
        */
    }
}