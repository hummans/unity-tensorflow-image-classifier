using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cameraTarget;
    private Quaternion _offset;

    private void Awake() {
        cameraTarget = GameObject.Find("AR Camera").GetComponent<Camera>();
        _offset = transform.rotation;
    }
    void Update()
    {
        // vector pointing from the camera towards the player
        var targetDirection = cameraTarget.transform.position - transform.position;
        targetDirection.y = 0;
        this.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up) * _offset;
    }
}
