using System.Collections;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    protected static bool isWolf = true;  // Default to Wolf
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private float transformDuration = 5f;  // Durasi transformasi
    [SerializeField] private float cooldownDuration = 10f;  // Durasi cooldown
    private bool isOnCooldown = false;

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isOnCooldown)
        {
            StartCoroutine(Transforming());
        }
    }

    private IEnumerator Transforming()
    {
        // Mulai transformasi menjadi domba
        isWolf = false;
        wolfPrefab.SetActive(false);
        sheepPrefab.SetActive(true);
        Debug.Log("Menjadi mbe");

        // Menunggu durasi transformasi
        yield return new WaitForSeconds(transformDuration);

        // Kembali menjadi serigala
        isWolf = true;
        wolfPrefab.SetActive(true);
        sheepPrefab.SetActive(false);
        Debug.Log("Kembali menjadi Wolf");

        // Mulai cooldown
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
        Debug.Log("Skill is colldown!");
    }
}
