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

    public Mesh sphereMesh; // Mesh sphere yang akan digunakan
    public Material sphereMaterial; // Material untuk sphere
    public float transformDuration = 5f; // Durasi transformasi menjadi sphere
    public float skillCooldown = 15f; // Cooldown skill transformasi
    private bool isCooldown = false; // Flag untuk melacak cooldown

    public Vector3 sphereScale = new Vector3(2f, 2f, 2f); // Ukuran sphere yang diinginkan
    private Mesh originalMesh; // Menyimpan mesh asli serigala
    private Material[] originalMaterials; // Menyimpan material asli serigala
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        originalMoveSpeed = moveSpeed;
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false;
        }

        // Mendapatkan referensi ke komponen MeshFilter dan MeshRenderer
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Cek apakah MeshFilter ada
        if (meshFilter != null)
        {
            originalMesh = meshFilter.mesh;
        }
        else
        {
            Debug.LogError("MeshFilter component is missing on this GameObject.");
        }

        // Cek apakah MeshRenderer ada
        if (meshRenderer != null)
        {
            originalMaterials = meshRenderer.materials;
        }
        else
        {
            Debug.LogError("MeshRenderer component is missing on this GameObject.");
        }
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
                TransformToSphere();
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

    public void TransformToSphere()
    {
        if (!isCooldown)
        {
            Debug.Log("Attempting to transform into sphere...");

            // Ganti mesh dan material serigala dengan sphere
            if (meshFilter != null && sphereMesh != null)
            {
                meshFilter.mesh = sphereMesh;
                transform.localScale = sphereScale; // Atur ukuran sphere
            }
            else
            {
                Debug.LogError("MeshFilter or SphereMesh is missing!");
                return;
            }

            if (meshRenderer != null && sphereMaterial != null)
            {
                meshRenderer.material = sphereMaterial;
            }
            else
            {
                Debug.LogError("MeshRenderer or SphereMaterial is missing!");
            }

            Debug.Log("Player transformed into a sphere!");

            StartCoroutine(RevertToWolfAfterDuration());
            StartCoroutine(TransformCooldown());
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    private IEnumerator RevertToWolfAfterDuration()
    {
        yield return new WaitForSeconds(transformDuration);

        // Kembalikan mesh dan material asli serigala
        if (meshFilter != null)
        {
            meshFilter.mesh = originalMesh;
        }
        if (meshRenderer != null)
        {
            meshRenderer.materials = originalMaterials;
        }

        // Kembalikan ukuran asli
        transform.localScale = Vector3.one; // Ganti dengan ukuran asli jika berbeda

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
