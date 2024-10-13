using UnityEngine;

public class Sheep : MonoBehaviour
{
    public bool isEaten = false;

    public void OnEaten()
    {
        // Tambahkan efek atau animasi saat domba dimakan
        Debug.Log("Sheep has been eaten!");
    }
}
