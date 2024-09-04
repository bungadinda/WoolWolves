using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Gameplay gameplay;
    private bool isMovable = true;
    private float originalMoveSpeed;
    public Image dirtyScreenEffect;
    public bool isHidden { get; private set; }


    private BushHiding currentBush; // Referensi ke script BushHiding

    private void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        originalMoveSpeed = moveSpeed;
        isHidden = false;
        if (dirtyScreenEffect != null)
        {
            dirtyScreenEffect.enabled = false;
        }
    }

    private void Update()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move.magnitude >= 0.1f)
        {
            transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void RotatePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveX, moveZ) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bush"))
        {
            currentBush = other.GetComponent<BushHiding>();
            if (currentBush != null)
            {
                currentBush.ToggleHide(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bush") && currentBush != null && other.GetComponent<BushHiding>() == currentBush)
        {
            currentBush.ToggleHide(false);
            currentBush = null;
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

    public void SetHidden(bool hiddenStatus)
    {
        isHidden = hiddenStatus;
    }

}
