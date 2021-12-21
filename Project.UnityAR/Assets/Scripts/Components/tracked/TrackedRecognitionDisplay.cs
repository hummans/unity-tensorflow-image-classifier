using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;
using UniRx;
using System;
using TMPro;
using System.Threading.Tasks;

namespace Components
{
    public class TrackedRecognitionDisplay : MonoBehaviour
    {
        public TrackManagerViewModel trackImageManager;
        public TextMeshProUGUI recognitionLabel;
        public Animator animator;

        private string _defaultText = "It's a";

        void Start()
        {
            trackImageManager.currentRecognition.Value = -1;
            trackImageManager.currentRecognition
                .Subscribe(OnRecognition)
                .AddTo(this);
        }

        private void OnRecognition(int i)
        {
            if(i > trackImageManager.deepLearningConfig.recognitionResponse.Length - 1 || i == -1)
                return;
            
            string value = trackImageManager.deepLearningConfig.recognitionResponse[i];
            string text = _defaultText + $" {value}";

            recognitionLabel.text = text;

            animator.SetTrigger("Open");

            Invoke("OffLabel", 3);
        }

        private void OffLabel()
        {
            recognitionLabel.text = "...";
            trackImageManager.currentRecognition.Value = -1;
        }
    }
}
