 using UnityEngine;
 using System.Collections;
using Managers;
using TMPro;

public class ConsoleApplicationLogInput : MonoBehaviour
{
   void OnEnable () => Application.logMessageReceived += OnLogHandler;
   void OnDisable () => Application.logMessageReceived -= OnLogHandler;
   
   private void OnLogHandler(string logString, string stackTrace, LogType type)
   {
       if(type == LogType.Warning)
            return;
       
       ConsoleClass.Instance.consoleData.logInput.Value = new LogData{
           type = type.ToString(),
           body = logString
       };
   }
}