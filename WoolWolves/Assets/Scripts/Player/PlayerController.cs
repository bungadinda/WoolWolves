using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isHidden = false;
    private Gameplay gameplay;
    private bool isMovable = true;
    private float originalMoveSpeed;
    public Image dirtyScreenEffect;

    private Material bushMaterial; // Material dari bush yang saat ini disentuh oleh player
    private Collider currentBushCollider; // Collider dari bush yang sedang disentuh oleh player

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        originalMoveSpeed = moveSpeed;
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false;
        }
    }

    private void Update()
    {
        // if (isMovable)
        // {
        //     if (!isHidden)
        //     {
        //         Move();
        //     }

        //     if (Input.GetKeyDown(KeyCode.F))
        //     {
        //         ToggleHide();
        //     }
        // }
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized; // Normalisasi untuk mendapatkan arah yang benar

        // Jika ada input arah, lakukan rotasi
        if (move.magnitude >= 0.1f)
        {
            // Tentukan sudut rotasi dari arah yang dituju
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;

            // Rotasi player ke arah yang dituju
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Gerakkan player ke arah yang dituju
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        }
    }


    private void ToggleHide()
    {
        isHidden = !isHidden;
        Debug.Log(isHidden ? "Player is now hiding" : "Player is no longer hiding");
        // Ubah transparansi hanya pada bush yang sedang ditabrak
    }

    private void SetBushTransparency(bool isHidden)
    {
        if (bushMaterial != null)
        {
            if (isHidden)
            {
                bushMaterial.SetFloat("_Surface", 1); // 1 untuk Transparent
                bushMaterial.SetOverrideTag("RenderType", "Transparent");
                bushMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                Color color = bushMaterial.color;
                color.a = 0.3f; // Set transparansi
                bushMaterial.color = color;
            }
            else
            {
                bushMaterial.SetFloat("_Surface", 0); // 0 untuk Opaque
                bushMaterial.SetOverrideTag("RenderType", "Opaque");
                bushMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                Color color = bushMaterial.color;
                color.a = 1f; // Kembalikan ke tidak transparan
                bushMaterial.color = color;
            }
        }
        else
        {
            Debug.LogError("Bush material is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bush"))
        {
            Debug.Log("Entered bush trigger. Bush name: " + other.name);
            ToggleHide();
            currentBushCollider = other; // Simpan referensi collider bush
            bushMaterial = other.GetComponent<Renderer>().material; // Simpan referensi material bush
            SetBushTransparency(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bush") && other == currentBushCollider)
        {
            Debug.Log("Exited bush trigger. Bush name: " + other.name);
            ToggleHide();
            // Kembalikan bush ke kondisi awal saat keluar dari bush
            SetBushTransparency(false);
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("sheep"))
    //     {
    //         Debug.Log("Collided with sheep");
    //         Destroy(collision.gameObject);
    //         gameplay.EatSheep();
    //     }
    //     else if (collision.gameObject.CompareTag("DombaSiluman"))
    //     {
    //         Debug.Log("Collided with DombaSiluman");
    //         Destroy(collision.gameObject);
    //         ApplySlowEffect();
    //         MakeScreenDirty();
    //         NotifyShepherd();
    //     }
    // }

    public void ApplySlowEffect()
    {
        Debug.Log("ApplySlowEffect called");
        moveSpeed /= 2;
        Invoke("RemoveSlowEffect", 4f);
    }

    private void RemoveSlowEffect()
    {
        Debug.Log("RemoveSlowEffect called");
        moveSpeed = originalMoveSpeed;
    }

    public void MakeScreenDirty()
    {
        Debug.Log("MakeScreenDirty called");
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = true;
            Invoke("ClearDirtyScreen", 4f);
        }
    }

    private void ClearDirtyScreen()
    {
        Debug.Log("ClearDirtyScreen called");
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false;
        }
    }

    public void SetMovable(bool movable)
    {
        isMovable = movable;
    }

    public void NotifyShepherd()
    {
        Debug.Log("NotifyShepherd called");
        EnemyAI shepherd = FindObjectOfType<EnemyAI>();
        if (shepherd != null)
        {
            shepherd.ChasePlayer(transform);
        }
    }
}
