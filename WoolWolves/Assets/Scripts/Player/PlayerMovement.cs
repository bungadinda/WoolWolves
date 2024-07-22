using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static Vector3 playerPos;
    private float speed = 10f;
    private Rigidbody rb;
    private Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(TrackPlayer());
    }

    IEnumerator TrackPlayer()
    {
        while(true)
        {
            playerPos = transform.position;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0, moveVertical);
    }

    void FixedUpdate()
    {
        // Moving the player
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }
}
