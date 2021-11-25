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
    public class PlayTurnGateway : IPlayGateway
    {
        private protected string URL_DATA = "https://ua6hsarvmg.execute-api.us-east-2.amazonaws.com/post_url_colormapping";
        
        public IObservable<Unit> PlayTurn(TrackManager trackData, Texture2D textureScreenshoot)
        {
            return Observable.FromCoroutine<Unit>(observer => GetImageColor(observer, trackData, textureScreenshoot));
        }

        IEnumerator GetImageColor(IObserver<Unit> observer, TrackManager trackData, Texture2D textureScreenshoot)
        {
            // Get Post S3 Url
            string id = Random.Range(0,1000000).ToString();                
            string datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string postUrl = URL_DATA + $"?expiresin=120&key={id}_unity_colormapping_{datetime}.png";

            UnityWebRequest wwwGetPostUrl = UnityWebRequest.Get(postUrl);

            yield return wwwGetPostUrl.SendWebRequest();

            trackData.currentTrackLabel.Value = "[RGB] Uploading image..";

            if(wwwGetPostUrl.result == UnityWebRequest.Result.ConnectionError || wwwGetPostUrl.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("UnityWebRequest: " + wwwGetPostUrl.error);
                Debug.LogError("UnityWebError: " + JSON.Parse(wwwGetPostUrl.downloadHandler.text));
                observer.OnNext(Unit.Default); // push Unit or all buffer result.
                observer.OnCompleted();
                yield break;
            }    

            JSONNode S3Url = JSON.Parse(wwwGetPostUrl.downloadHandler.text);
            string POST_URL = S3Url["url"];
            Debug.Log($"[PlayTurnGateway] S3 Post Url is {POST_URL}");

            // Post S3 Image
            JSONNode fields = S3Url["fields"];
            string file_name = fields["key"];

            Debug.Log($"New file uploading name is {file_name}");

            WWWForm form = AWSWebFormBuilder.BuildUploadRequest(fields, textureScreenshoot, file_name);

            UnityWebRequest wwwPostImage = UnityWebRequest.Post(POST_URL, form);

            yield return wwwPostImage.SendWebRequest();

            if(wwwPostImage.result == UnityWebRequest.Result.ConnectionError || wwwPostImage.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("UnityWebRequest: " + wwwPostImage.error);
                Debug.LogError("UnityWebError: " + JSON.Parse(wwwPostImage.downloadHandler.text));
                observer.OnNext(Unit.Default); // push Unit or all buffer result.
                observer.OnCompleted();
                yield break;
            }    

/*
            JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);

            string pokeName = pokeInfo["name"];
            string pokeSpriteUrl = pokeInfo["sprites"]["front_default"];
            JSONNode pokeTypes = pokeInfo["types"];
            string[] pokeTypeNames = new string[pokeTypes.Count];

            for (int i = 0, j = pokeTypes.Count - 1; i < pokeTypes.Count; i++, j--)
            {
                pokeTypeNames[j] = pokeTypes[i]["type"]["name"];
            }

            UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteUrl);
            yield return pokeSpriteRequest.SendWebRequest();
*/
            ColorObject colorObject = GenerateColor(5);

            trackData.ColorObject.Value = colorObject;
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