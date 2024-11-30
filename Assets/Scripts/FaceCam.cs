using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCam : MonoBehaviour
{
    public Transform canvasTransform;
    float InitialCameraSize;
    Vector3 initialUiScale;

    void Start()
    {
        InitialCameraSize = Camera.main.orthographicSize;
        initialUiScale = transform.localScale;
    }

    void LateUpdate()
    {
        canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
        transform.localScale = initialUiScale * Camera.main.orthographicSize / InitialCameraSize;
    }
}
