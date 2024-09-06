using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 3.0f; // Jarak deteksi untuk objek dengan tag "sheep"
    public Gameplay gameplay; // Referensi ke script Gameplay
    public AlarmSystem alarmSystem; // Referensi ke script AlarmSystem
    public Button eatSheepButton; // Referensi ke button EatSheep

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>(); // Cari objek dengan script Gameplay
        alarmSystem = FindObjectOfType<AlarmSystem>(); // Cari objek dengan script AlarmSystem

        if (eatSheepButton != null)
        {
            eatSheepButton.onClick.AddListener(DestroyNearbySheepButton); // Tambahkan listener untuk button
        }
    }

    void Update()
    {
        // Memeriksa input dari tombol Space untuk menghancurkan sheep
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyNearbySheep(); // Panggil fungsi untuk menghancurkan sheep
            if (eatSheepButton != null)
            {
                StartCoroutine(ButtonPressEffect()); // Efek pada button
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

                // Pastikan gameplay tidak null sebelum memanggil EatSheep
                if (gameplay != null)
                {
                    gameplay.EatSheep();
                }
                Destroy(sheep);

                // Pastikan alarmSystem tidak null sebelum memanggil TriggerAlarm
                if (alarmSystem != null)
                {
                    alarmSystem.TriggerAlarm(deathLocation);
                }
            }
        }

        // Memproses objek dengan tag "DombaSiluman"
        foreach (GameObject dombaSiluman in dombaSilumanObjects)
        {
            float distanceToDombaSiluman = Vector3.Distance(transform.position, dombaSiluman.transform.position);

            if (distanceToDombaSiluman <= detectionRange)
            {
                Destroy(dombaSiluman);

                // Memastikan playerController tidak null
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

    // Fungsi untuk button "Eat Sheep"
    public void DestroyNearbySheepButton()
    {
        DestroyNearbySheep(); // Panggil fungsi yang sama dengan input Space
    }

    // Coroutine untuk efek button ketika ditekan
    private IEnumerator ButtonPressEffect()
    {
        if (eatSheepButton != null)
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
}
