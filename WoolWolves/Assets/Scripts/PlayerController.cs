using UnityEngine;
using UnityEngine.UI; // Untuk mengakses UI

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isHidden = false; // Status sembunyi
    private Gameplay gameplay;
    private bool isMovable = true; // Status untuk mengontrol gerakan pemain
    private float originalMoveSpeed; // Untuk menyimpan kecepatan asli
    public Image dirtyScreenEffect; // UI Image untuk efek kotor

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        originalMoveSpeed = moveSpeed; // Simpan kecepatan asli
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false; // Nonaktifkan Image pada awalnya
        }
    }

    private void Update()
    {
        if (isMovable)
        {
            if (!isHidden) // Hanya izinkan gerakan jika tidak tersembunyi
            {
                Move();
            }

            // Cek input tombol F
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleHide();
            }
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
        else if (collision.gameObject.CompareTag("DombaSiluman"))
        {
            Debug.Log("Collided with DombaSiluman");
            Destroy(collision.gameObject);
            ApplySlowEffect();
            MakeScreenDirty();
            NotifyShepherd();
        }
    }

    public void ApplySlowEffect() // Ubah akses menjadi public
    {
        Debug.Log("ApplySlowEffect called");
        moveSpeed /= 2; // Kurangi kecepatan
        Invoke("RemoveSlowEffect", 4f); // Kembalikan kecepatan setelah 4 detik
    }

    private void RemoveSlowEffect()
    {
        Debug.Log("RemoveSlowEffect called");
        moveSpeed = originalMoveSpeed; // Kembalikan kecepatan asli
    }

    public void MakeScreenDirty() // Ubah akses menjadi public
    {
        Debug.Log("MakeScreenDirty called");
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = true; // Aktifkan efek kotor
            Invoke("ClearDirtyScreen", 4f); // Hapus efek kotor setelah 4 detik
        }
    }

    private void ClearDirtyScreen()
    {
        Debug.Log("ClearDirtyScreen called");
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false; // Nonaktifkan efek kotor
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

    // Method untuk mengatur status gerakan
    public void SetMovable(bool movable)
    {
        isMovable = movable;
    }

    public void NotifyShepherd() // Ubah akses menjadi public
    {
        Debug.Log("NotifyShepherd called");
        EnemyAI shepherd = FindObjectOfType<EnemyAI>();
        if (shepherd != null)
        {
            shepherd.ChasePlayer(transform);
        }
    }
}
