using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 5.0f; // Periksa jarak deteksi
    public Gameplay gameplay; // Referensi ke script Gameplay untuk menghitung domba yang dimakan
    public AlarmSystem alarmSystem; // Referensi ke AlarmSystem untuk mengaktifkan alarm
    public Button eatSheepButton; // Tombol untuk memakan domba
    public ParticleSystem slowEffect; // Efek VFX saat domba siluman dimakan
    public float slowDuration = 4f; // Durasi efek slow

    void Start()
    {
        // Cari script Gameplay dan AlarmSystem di scene
        gameplay = FindObjectOfType<Gameplay>();
        alarmSystem = FindObjectOfType<AlarmSystem>();

        if (gameplay == null)
        {
            Debug.LogError("Gameplay script not found!");
        }

        if (alarmSystem == null)
        {
            Debug.LogWarning("AlarmSystem not found in the scene.");
        }

        if (eatSheepButton != null)
        {
            eatSheepButton.onClick.AddListener(DestroyNearbySheepButton);
        }
    }

    void Update()
    {
        // Tombol Space untuk memakan domba
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyNearbySheep();
            if (eatSheepButton != null)
            {
                StartCoroutine(ButtonPressEffect());
            }
        }
    }

    // Fungsi utama untuk memakan domba di sekitar
    public void DestroyNearbySheep()
    {
        // Cari semua objek domba biasa
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("sheep");
        GameObject[] dombaSilumanObjects = GameObject.FindGameObjectsWithTag("DombaSiluman");

        Debug.Log("Attempting to destroy sheep. Sheep count: " + sheepObjects.Length);

        // Hancurkan domba biasa
        foreach (GameObject sheepObject in sheepObjects)
        {
            float distanceToSheep = Vector3.Distance(transform.position, sheepObject.transform.position);

            if (distanceToSheep <= detectionRange)
            {
                Debug.Log("Sheep within range!");

                if (gameplay != null)
                {
                    Sheep sheepComponent = sheepObject.GetComponent<Sheep>();
                    if (sheepComponent != null && !sheepComponent.isEaten)
                    {
                        gameplay.EatSheep(); // Tambahkan jumlah domba yang dimakan
                        Debug.Log("Sheep eaten, updating counter.");
                        sheepComponent.isEaten = true; // Tandai domba sebagai sudah dimakan
                        sheepComponent.OnEaten(); // Panggil efek visual atau suara
                    }
                    else
                    {
                        Debug.LogWarning("Sheep component not found or sheep already eaten.");
                    }
                }
                else
                {
                    Debug.LogError("Gameplay script is missing!");
                }

                Destroy(sheepObject); // Hancurkan domba

                if (alarmSystem != null)
                {
                    alarmSystem.TriggerAlarm(sheepObject.transform.position); // Aktifkan alarm di lokasi domba dimakan
                }
            }
            else
            {
                Debug.Log("Sheep too far away.");
            }
        }

        // Hancurkan domba siluman
        foreach (GameObject dombaSiluman in dombaSilumanObjects)
        {
            float distanceToDombaSiluman = Vector3.Distance(transform.position, dombaSiluman.transform.position);

            if (distanceToDombaSiluman <= detectionRange)
            {
                Debug.Log("Domba Siluman within range!");

                // Tidak menambah countersheep untuk domba siluman
                Destroy(dombaSiluman); // Hancurkan domba siluman

                if (alarmSystem != null)
                {
                    alarmSystem.TriggerAlarm(dombaSiluman.transform.position); // Aktifkan alarm di lokasi domba siluman
                }

                PlayerController playerController = GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ApplySlowEffect(); // Efek slow ke pemain
                    playerController.MakeScreenDirty(); // Munculkan efek layar kotor
                    playerController.NotifyShepherd(); // Notifikasi ke pemain
                }

                // Aktifkan efek slow VFX
                if (slowEffect != null)
                {
                    StartCoroutine(TriggerSlowEffect());
                }
            }
            else
            {
                Debug.Log("Domba Siluman too far away.");
            }
        }

        Debug.Log("Nearby sheep and domba siluman processed.");
    }

    // Fungsi untuk tombol "Eat Sheep"
    public void DestroyNearbySheepButton()
    {
        DestroyNearbySheep();
    }

    // Coroutine untuk memberikan efek ke tombol
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
