 using UnityEngine;
 using System.Collections;
using Managers;
using TMPro;
using System;

public class ConsoleApplicationLogInput : MonoBehaviour
{
   void OnEnable () => Application.logMessageReceived += OnLogHandler;
   void OnDisable () => Application.logMessageReceived -= OnLogHandler;
   
   private void OnLogHandler(string logString, string stackTrace, LogType type)
   {
       ConsoleClass.Instance.consoleData.logInput.Value = new LogData{
           time = DateTime.Now.ToString("HH:mm:ss"),
           type = type,
           body = logString
       };
   }
}