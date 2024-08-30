using UnityEngine;
using UnityEngine.UI; // Tambahkan namespace UI
using System.Collections;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 3.0f; // Jarak dalam unit Unity di mana player dapat mendeteksi objek dengan tag "sheep"
    public Gameplay gameplay; // Referensi ke script Gameplay
    public AlarmSystem alarmSystem;
    public Button eatSheepButton; // Tambahkan referensi ke Button

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>(); // Cari objek dengan script Gameplay
        alarmSystem = FindObjectOfType<AlarmSystem>(); // Cari objek dengan script AlarmSystem
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyNearbySheep(); // Panggil fungsi untuk menghancurkan domba saat tombol Space ditekan
            if (eatSheepButton != null)
            {
                StartCoroutine(ButtonPressEffect()); // Jalankan coroutine untuk efek button
            }
        }
    }

    public void DestroyNearbySheep()
    {
        // Mencari semua objek dengan tag "sheep"
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("sheep");
        GameObject[] dombaSilumanObjects = GameObject.FindGameObjectsWithTag("DombaSiluman");

        // Memproses objek dengan tag "sheep"
        foreach (GameObject sheep in sheepObjects)
        {
            float distanceToSheep = Vector3.Distance(transform.position, sheep.transform.position);

            if (distanceToSheep <= detectionRange)
            {
                Vector3 deathLocation = sheep.transform.position;
                gameplay.EatSheep();
                Destroy(sheep);
                alarmSystem.TriggerAlarm(deathLocation);
            }
        }

        // Memproses objek dengan tag "DombaSiluman"
        foreach (GameObject dombaSiluman in dombaSilumanObjects)
        {
            float distanceToDombaSiluman = Vector3.Distance(transform.position, dombaSiluman.transform.position);

            if (distanceToDombaSiluman <= detectionRange)
            {
                Destroy(dombaSiluman);
                PlayerController playerController = GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ApplySlowEffect();
                    playerController.MakeScreenDirty();
                    playerController.NotifyShepherd();
                }
            }
        }
    }

    // Fungsi untuk button
    public void DestroyNearbySheepButton()
    {
        
    }

    // Coroutine untuk memberikan efek button
    private IEnumerator ButtonPressEffect()
    {
        var buttonColors = eatSheepButton.colors;
        Color originalColor = buttonColors.normalColor;
        buttonColors.normalColor = buttonColors.pressedColor;
        eatSheepButton.colors = buttonColors;

        yield return new WaitForSeconds(0.1f); // Durasi efek pressed color

        buttonColors.normalColor = originalColor;
        eatSheepButton.colors = buttonColors;
    }
}
