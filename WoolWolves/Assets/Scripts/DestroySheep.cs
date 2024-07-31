using UnityEngine;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 3.0f; // Jarak dalam unit Unity di mana player dapat mendeteksi objek dengan tag "sheep"
    public Gameplay gameplay; // Referensi ke script Gameplay
    public AlarmSystem alarmSystem;
    

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>(); // Cari objek dengan script Gameplay
        alarmSystem = FindObjectOfType<AlarmSystem>(); // referensii ke game object yang ada script alarm system
    }

    void Update()
    {
        // Mencari semua objek dengan tag "sheep"
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("sheep");
        GameObject[] dombaSilumanObjects = GameObject.FindGameObjectsWithTag("DombaSiluman");

        // Memproses objek dengan tag "sheep"
        foreach (GameObject sheep in sheepObjects)
        {
            float distanceToSheep = Vector3.Distance(transform.position, sheep.transform.position);

            if (distanceToSheep <= detectionRange && Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 deathLocation = sheep.transform.position;
                Destroy(sheep);
                alarmSystem.TriggerAlarm(deathLocation);
                gameplay.EatSheep();
            }
        }

        // Memproses objek dengan tag "DombaSiluman"
        foreach (GameObject dombaSiluman in dombaSilumanObjects)
        {
            float distanceToDombaSiluman = Vector3.Distance(transform.position, dombaSiluman.transform.position);

            if (distanceToDombaSiluman <= detectionRange && Input.GetKeyDown(KeyCode.Space))
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
}
