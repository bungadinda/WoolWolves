using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    Vector3 offset;
    float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 25, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Tentukan posisi yang diinginkan untuk kamera
        Vector3 targetPosition = playerTransform.position + offset;
        // Gunakan Lerp untuk membuat gerakan kamera lebih halus
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        // Atur posisi kamera
        transform.position = smoothedPosition;
    }
}
