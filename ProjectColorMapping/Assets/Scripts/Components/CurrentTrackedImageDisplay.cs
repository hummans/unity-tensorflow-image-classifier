using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTrackedImageDisplay : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("TurnOff", 3f);
    }
    void TurnOff()
    {
        this.gameObject.GetComponent<RawImage>().texture = null;
        this.gameObject.SetActive(false);
    }
}
