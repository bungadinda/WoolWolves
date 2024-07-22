using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Gameplay gameplay;

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sheep"))
        {
            Debug.Log("Collided with sheep");
            Destroy(collision.gameObject);
            gameplay.EatSheep();
        }
    }


}