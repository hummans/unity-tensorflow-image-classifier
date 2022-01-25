using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using ViewModel;
using Random = UnityEngine.Random;
using SimpleJSON;

namespace Infrastructure
{
    public class PlayTurnGateway : IPlayGateway
    {
        private protected string URL_DATA = "https://ua6hsarvmg.execute-api.us-east-2.amazonaws.com/";
        private protected const string _postUrl = "post_url_colormapping";
        private protected const string _classifierUrl = "cat_dog_classifier";

        public IObservable<bool> PlayTurn(TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            return Observable.FromCoroutine<bool>(observer => PlayImage(observer, trackData, textureScreenshoot));
        }

        IEnumerator PlayImage(IObserver<bool> observer, TrackManagerViewModel trackData, Texture2D textureScreenshoot)
        {
            // Play input function
            // 1. Get post url with credentials
            // 2. Post image to s3
            // 3. Get image recognition values ({'0': Cat, '1': Dog})
            // 4. Save API Response
            
            // Step 1
            trackData.currentTrackLabel.Value = "[Play] Prepare image..";

            string id = Random.Range(0,100001).ToString();                
            string datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string postUrl = URL_DATA + _postUrl + $"?expiresin=120&key={id}_unity_colormapping_{datetime}.png";

            UnityWebRequest wwwGetPostUrl = UnityWebRequest.Get(postUrl);
            yield return wwwGetPostUrl.SendWebRequest();

            if(wwwGetPostUrl.result == UnityWebRequest.Result.ConnectionError || wwwGetPostUrl.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("UnityWebRequest: " + wwwGetPostUrl.error);
                observer.OnNext(false); // push Unit or all buffer result.
                observer.OnError(new UnityException());
                yield break;
            }    
            
            JSONNode S3Url = JSON.Parse(wwwGetPostUrl.downloadHandler.text);
            string POST_URL = S3Url["url"];
          
           // Step 2
           // JSONNode fields = S3Url["fields"];
           // string file_name = fields["key"];
           // Debug.Log($"New file uploading name is {file_name}");//

           // WWWForm form = AWSWebFormBuilder.BuildUploadRequest(fields, textureScreenshoot, file_name);
           // UnityWebRequest wwwPostImage = UnityWebRequest.Post(POST_URL, form);
           // yield return wwwPostImage.SendWebRequest();//

           // if(wwwPostImage.result == UnityWebRequest.Result.ConnectionError || wwwPostImage.result == UnityWebRequest.Result.ProtocolError)
           // {
           //     Debug.LogError("UnityWebRequest: " + wwwPostImage.error);
           //     observer.OnNext(Unit.Default); // push Unit or all buffer result.
           //     observer.OnError(new UnityException());
           //     yield break;
           // }    
        
            // Step 3
            trackData.currentTrackLabel.Value = "[Play] Uploading image..";

            string image64 = Convert.ToBase64String(textureScreenshoot.EncodeToJPG());
            string image_url = POST_URL;

            postUrl = URL_DATA + _classifierUrl + $"?image_base64={image64}&image_url={image_url}";
            
            UnityWebRequest wwwGetCatDog = UnityWebRequest.Get(postUrl);
            wwwGetCatDog.SetRequestHeader("AuthorizationToken", "C2hESPaO");
            yield return wwwGetCatDog.SendWebRequest();

            if(wwwGetPostUrl.result == UnityWebRequest.Result.ConnectionError || wwwGetPostUrl.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("UnityWebRequest: " + wwwGetPostUrl.error);
                observer.OnNext(false); // push Unit or all buffer result.
                observer.OnError(new UnityException());
                yield break;
            }              
            
            JSONNode classifier = JSONNode.Parse(wwwGetCatDog.downloadHandler.text);
            JSONNode body = JSONNode.Parse(classifier["Body"]);
            JSONNode response = JSONNode.Parse(body["Response"]);

            trackData.currentTrackLabel.Value = "[Play] Classifying image..";
            Debug.Log($"{response["label"]} with {response["accuracy"]} of accuracy");

            // Step 4
            trackData.currentRecognition.Value = new RecognitionResponse(){
                prediction = Convert.ToInt32(response["prediction"].ToString()),
                accuracy = Convert.ToSingle(response["accuracy"].ToString()),
                label = response["label"].ToString()
            };
            
            observer.OnNext(true); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}