using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform[] targets;
    [Range(0.01f,1)]
    public float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;

    private void Update()
    {
        Vector3 v3 = Vector3.zero;

        for (int i = 0; i < targets.Length; i++) { v3 += targets[i].position; }
        v3 /= targets.Length;

        transform.position = Vector3.SmoothDamp(transform.position, v3, ref velocity, smoothTime);
    }
}
