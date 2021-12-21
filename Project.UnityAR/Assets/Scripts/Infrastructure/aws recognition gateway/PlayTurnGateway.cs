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
        private protected string URL_DATA = "https://ua6hsarvmg.execute-api.us-east-2.amazonaws.com/";
        private protected const string _postUrl = "post_url_colormapping";
        private protected const string _classifierUrl = "cat_dog_classifier";

        public IObservable<Unit> PlayTurn(TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            return Observable.FromCoroutine<Unit>(observer => PlayImage(observer, trackData, textureScreenshoot));
        }

        IEnumerator PlayImage(IObserver<Unit> observer, TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            // Play input function
            // 1. Get post url with credentials
            // 2. Post image to s3
            // 3. Get image recognition values ({'0': Cat, '1': Dog})
            // 4. Save API Response

            string id = Random.Range(0,100001).ToString();                
            string datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string postUrl = URL_DATA + _postUrl + $"?expiresin=120&key={id}_unity_colormapping_{datetime}.png";

            trackData.currentTrackLabel.Value = "[Play] Uploading image..";
            UnityWebRequest wwwGetPostUrl = UnityWebRequest.Get(postUrl);
            yield return wwwGetPostUrl.SendWebRequest();

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
            Debug.Log($"s3 Post Url is {POST_URL}");

            JSONNode fields = S3Url["fields"];
            string file_name = fields["key"];
            Debug.Log($"New file uploading name is {file_name}");

            # region Upload
            /*
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
            # endregion
            
            string image64 = Convert.ToBase64String(textureScreenshoot.EncodeToJPG());
            string image_url = POST_URL;

            postUrl = URL_DATA + _classifierUrl + $"?image_base64={image64}&image_url={image_url}";
            
            trackData.currentTrackLabel.Value = "[Play] Analyzing image..";
            UnityWebRequest wwwGetCatDog = UnityWebRequest.Get(postUrl);
            yield return wwwGetCatDog.SendWebRequest();

            if(wwwGetPostUrl.result == UnityWebRequest.Result.ConnectionError || wwwGetPostUrl.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("UnityWebRequest: " + wwwGetPostUrl.error);
                Debug.LogError("UnityWebError: " + JSON.Parse(wwwGetPostUrl.downloadHandler.text));
                observer.OnNext(Unit.Default); // push Unit or all buffer result.
                observer.OnCompleted();
                yield break;
            }              

            JSONNode classifier = JSONNode.Parse(wwwGetCatDog.downloadHandler.text);
            JSONNode body = JSONNode.Parse(classifier["Body"]);
            JSONNode response = JSONNode.Parse(body["Response"]);

            Debug.Log($"{response["label"]} with {response["accuracy"]} of accuracy");
            trackData.currentRecognition.Value = response["prediction"];
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}