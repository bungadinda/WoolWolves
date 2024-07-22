using UnityEngine;

public class DestroySheep : MonoBehaviour
{
    public float detectionRange = 3.0f; // Jarak dalam unit Unity di mana player dapat mendeteksi objek dengan tag "sheep"
    public Gameplay gameplay; // Referensi ke script Gameplay

    void Start()
    {
        gameplay = FindObjectOfType<Gameplay>(); // Cari objek dengan script Gameplay
    }

    void Update()
    {
        // Mencari semua objek dengan tag "sheep"
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("sheep");

        foreach (GameObject sheep in sheepObjects)
        {
            // Menghitung jarak antara player dan objek dengan tag "sheep"
            float distanceToSheep = Vector3.Distance(transform.position, sheep.transform.position);

            // Jika jarak lebih kecil atau sama dengan detectionRange dan tombol space ditekan
            if (distanceToSheep <= detectionRange && Input.GetKeyDown(KeyCode.Space))
            {
                // Menghancurkan objek dengan tag "sheep"
                Destroy(sheep);

                // Panggil fungsi untuk menambah counter domba yang dimakan
                gameplay.EatSheep();
            }
        }
    }
}
