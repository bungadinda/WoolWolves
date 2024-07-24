using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isHidden = false; // Status sembunyi
    private Gameplay gameplay;

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
    }

    private void Update()
    {
        if (!isHidden) // Hanya izinkan gerakan jika tidak tersembunyi
        {
            Move();
        }

        // Cek input tombol J
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleHide();
        }
    }

    private void Move()
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

    private void ToggleHide()
    {
        if (IsNearBush())
        {
            isHidden = !isHidden;
            // Sesuaikan tampilan atau status lainnya jika perlu
            Debug.Log(isHidden ? "Player is now hiding" : "Player is no longer hiding");
        }
    }

    private bool IsNearBush()
    {
        // Implementasikan deteksi jarak dengan semak-semak di sini
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Jarak deteksi
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bush"))
            {
                return true;
            }
        }
        return false;
    }
}
