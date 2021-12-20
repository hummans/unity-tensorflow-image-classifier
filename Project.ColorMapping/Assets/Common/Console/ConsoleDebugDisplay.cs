using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewModel;
using UniRx;
using System;
using TMPro;

public class ConsoleDebugDisplay : MonoBehaviour
{
    public ConsoleViewModel debugConsole;
    public TextMeshProUGUI consoleLabel;

    private int _current;
    
    void Awake() 
    {
        _current = debugConsole.maxToDisplay;

        debugConsole.logInput.Value = new LogData(){
            type = LogType.Log,
            body = ""
        };

        debugConsole.logInput
            .Subscribe(OnConsoleChange)
            .AddTo(this); 
    }


    private void OnConsoleChange(LogData logData)
    {
        if(logData.body == "")
            return;
        
        switch(logData.type)
        {
            case LogType.Error:
            {
                if(debugConsole.showLogError)
                    ShowLog(logData);
                break;
            }
            case LogType.Warning:
            {
                if(debugConsole.showLogWarning)
                    ShowLog(logData);
                break;
            }
            case LogType.Log:
            {
                if(debugConsole.showLogMessage)
                    ShowLog(logData);
                break;
            }
        }
    }

    void ShowLog(LogData logData)
    {
        string type = logData.type.ToString();
        string log = $"[{type.Substring(0,1).ToUpper()}][{logData.time}] " + logData.body;
        string current = consoleLabel.text;

        if(_current == debugConsole.maxToDisplay)
        {
            consoleLabel.text = log;
            _current = 0;          
        } 
        else 
        {
            _current++;
            consoleLabel.text = log + "\n" + current + "\n";
        }
    }
}
