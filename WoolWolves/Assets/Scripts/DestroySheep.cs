using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 3.0f;
    public Gameplay gameplay;
    public AlarmSystem alarmSystem;
    public Button eatSheepButton;
    public ParticleSystem slowEffect; // Referensi untuk efek slow VFX
    public float slowDuration = 4f;   // Durasi slow

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>();
        alarmSystem = FindObjectOfType<AlarmSystem>();

        if (eatSheepButton != null)
        {
            eatSheepButton.onClick.AddListener(DestroyNearbySheepButton);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyNearbySheep();
            if (eatSheepButton != null)
            {
                StartCoroutine(ButtonPressEffect());
            }
        }
    }

    public void DestroyNearbySheep()
    {
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("sheep");
        GameObject[] dombaSilumanObjects = GameObject.FindGameObjectsWithTag("DombaSiluman");

        // Hancurkan domba normal
        foreach (GameObject sheepObject in sheepObjects)
        {
            float distanceToSheep = Vector3.Distance(transform.position, sheepObject.transform.position);

            if (distanceToSheep <= detectionRange)
            {
                Vector3 deathLocation = sheepObject.transform.position;

                // Pastikan gameplay bukan null
                    if (gameplay != null)
                    {
                        Sheep sheepComponent = sheepObject.GetComponent<Sheep>(); // Ambil komponen Sheep
                        if (sheepComponent != null && !sheepComponent.isEaten)
                        {
                            gameplay.EatSheep(); // Panggil fungsi EatSheep
                            sheepComponent.isEaten = true; // Tandai domba ini sudah dimakan
                            sheepComponent.OnEaten(); // Panggil efek saat domba dimakan
                        }
                    }
                

                Destroy(sheepObject); // Hapus objek domba

                if (alarmSystem != null)
                {
                    alarmSystem.TriggerAlarm(deathLocation);
                }
            }
        }

        // Hancurkan domba siluman
        foreach (GameObject dombaSiluman in dombaSilumanObjects)
        {
            float distanceToDombaSiluman = Vector3.Distance(transform.position, dombaSiluman.transform.position);

            if (distanceToDombaSiluman <= detectionRange)
            {
                Destroy(dombaSiluman); // Hapus objek domba siluman

                PlayerController playerController = GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ApplySlowEffect();
                    playerController.MakeScreenDirty();
                    playerController.NotifyShepherd();
                }

                // Aktifkan efek slow VFX
                if (slowEffect != null)
                {
                    Debug.Log("activate vfx");
                    StartCoroutine(TriggerSlowEffect());
                }
            }
        }
    }

    // Fungsi untuk button "Eat Sheep"
    public void DestroyNearbySheepButton()
    {
        DestroyNearbySheep();
    }

    private IEnumerator ButtonPressEffect()
    {
        if (eatSheepButton != null)
        {
            var buttonColors = eatSheepButton.colors;
            Color originalColor = buttonColors.normalColor;
            buttonColors.normalColor = buttonColors.pressedColor;
            eatSheepButton.colors = buttonColors;

            yield return new WaitForSeconds(0.1f);

            buttonColors.normalColor = originalColor;
            eatSheepButton.colors = buttonColors;
        }
    }

    // Coroutine untuk mengaktifkan dan menonaktifkan efek slow VFX
    private IEnumerator TriggerSlowEffect()
    {
        // Aktifkan GameObject yang berisi Particle System
        slowEffect.gameObject.SetActive(true);
        Debug.Log("Sfx Slow Effect Active");

        // Tunggu selama durasi slow
        yield return new WaitForSeconds(slowDuration);

        // Nonaktifkan GameObject yang berisi Particle System
        slowEffect.gameObject.SetActive(false);
        Debug.Log("Sfx Slow Effect Inactive");
    }
}
