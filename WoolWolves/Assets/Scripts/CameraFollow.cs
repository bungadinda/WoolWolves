using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Referensi ke Transform pemain
    [SerializeField] private Vector3 offset = new Vector3(0, 25, -10); // Offset dapat diatur melalui Inspector
    [SerializeField] private float smoothTime = 0.3f; // Juga dapat diatur melalui Inspector
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform tidak dihubungkan di Inspector.");
            return;
        }

        Vector3 targetPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;
    }
}
