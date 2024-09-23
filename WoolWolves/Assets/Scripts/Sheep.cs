using UnityEngine;

public class Sheep : MonoBehaviour
{
    public bool isEaten = false; // Menandakan apakah domba sudah dimakan

    // Fungsi ini bisa dipanggil untuk reset status jika diperlukan
    public void ResetSheep()
    {
        isEaten = false;
        // Tambahkan logika lain jika perlu
    }

    // Fungsi ini bisa dipanggil untuk melakukan efek saat domba dimakan
    public void OnEaten()
    {
        // Logika tambahan saat domba dimakan (efek suara, animasi, dll.)
        Debug.Log("Sheep has been eaten!");
        // Misalnya, bisa tambahkan animasi atau efek lain di sini
    }
}
