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

    //public GameObject sheepPrefab; // Prefab domba yang akan digunakan
    public float transformDuration = 5f; // Durasi transformasi menjadi domba
    public float skillCooldown = 15f; // Cooldown skill transformasi
    private bool isCooldown = false; // Flag untuk melacak cooldown

    private GameObject sheepInstance; // Instance dari prefab domba
    public GameObject originalWolf; // Menyimpan referensi ke objek asli serigala

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        originalMoveSpeed = moveSpeed;
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false;
        }

        // Menyimpan referensi ke game object serigala asli
        //originalWolf = this.gameObject;

        // Pastikan sheepPrefab tidak diaktifkan di awal
        /*
        if (sheepPrefab != null)
        {
            sheepInstance = Instantiate(sheepPrefab, transform.position, transform.rotation);
            sheepInstance.SetActive(false); // Pastikan ini hanya terjadi sekali, di awal
        }
        else
        {
            Debug.LogError("Sheep Prefab is missing!");
        }
        */
    }

    private void Update()
    {
        if (isMovable)
        {
            if (!isHidden)
            {
                Move();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleHide();
            }

            if (Input.GetKeyDown(KeyCode.T)) // Kunci untuk transformasi
            {
                //TransformToSheep();
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

    private void ToggleHide()
    {
        if (IsNearBush())
        {
            isHidden = !isHidden;
            Debug.Log(isHidden ? "Player is now hiding" : "Player is no longer hiding");
        }
    }

    private bool IsNearBush()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bush"))
            {
                return true;
            }
        }
        return false;
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

    /*
    public void TransformToSheep()
    {
        if (!isCooldown)
        {
            Debug.Log("Attempting to transform into sheep...");

            // Start coroutine first before deactivating the player
            StartCoroutine(RevertToWolfAfterDuration());

            // Menjadikan sheepInstance sebagai child dari originalWolf agar mengikuti transformasinya
            sheepInstance.transform.SetParent(originalWolf.transform);
            sheepInstance.transform.localPosition = Vector3.zero;
            sheepInstance.transform.localRotation = Quaternion.identity;

            // Matikan objek serigala dan aktifkan prefab domba
            originalWolf.SetActive(false);
            sheepInstance.SetActive(true);

            Debug.Log("Sheep instance is now active: " + sheepInstance.activeSelf); // Tambahkan ini

            Debug.Log("Player transformed into a sheep!");

            StartCoroutine(TransformCooldown());
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }
    */



    private IEnumerator RevertToWolfAfterDuration()
    {
        yield return new WaitForSeconds(transformDuration);

        // Matikan prefab domba dan aktifkan kembali objek serigala
        sheepInstance.SetActive(false);
        originalWolf.SetActive(true);

        Debug.Log("Player reverted to wolf!");
    }

    private IEnumerator TransformCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(skillCooldown);
        isCooldown = false;
        Debug.Log("Skill cooldown ended, can transform again.");
    }
}
