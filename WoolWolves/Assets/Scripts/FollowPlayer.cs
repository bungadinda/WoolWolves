using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 0);

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}
