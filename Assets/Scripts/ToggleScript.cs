﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{

    public enum ToggleVariable { Vibration, Protips};

    public ToggleVariable whichVariable;
    public Toggle onToggle;
    public Toggle offToggle;

    public bool toggleStatus = false;
    public Text labelText;

   
    // Start is called before the first frame update
    void Start()
    {
        //Load initial values from datamanager.
        switch (whichVariable)
        {
            case ToggleVariable.Vibration:
                toggleStatus = FileSystemLayer.Instance.vibrationOn == 1 ? true : false;
                break;

            case ToggleVariable.Protips:
                toggleStatus = FileSystemLayer.Instance.protipsOn == 1 ? true : false;
                break;

        }
     
        onToggle.isOn = toggleStatus;
        offToggle.isOn = !toggleStatus;
        //labelText.text = toggleStatus ? "ON" : "OFF";

    }

  
    public void Toggle()
    {
        toggleStatus = !toggleStatus;
        onToggle.isOn = toggleStatus;
        offToggle.isOn = !toggleStatus;
        //labelText.text = toggleStatus ? "ON" : "OFF";

        // Save Togglestatus. TODO: This should report to a filesystem manager to make sure the variables are saved to disk.
        switch (whichVariable)
        {
            case ToggleVariable.Vibration:
                FileSystemLayer.Instance.vibrationOn = toggleStatus ? 1 : 0;
                FileSystemLayer.Instance.SavePref("vibrationOn", FileSystemLayer.Instance.vibrationOn);
                break;

            case ToggleVariable.Protips:
                FileSystemLayer.Instance.protipsOn = toggleStatus ? 1 : 0;
                FileSystemLayer.Instance.SavePref("protipsOn", FileSystemLayer.Instance.protipsOn);
                break; 

        }
    }
}
