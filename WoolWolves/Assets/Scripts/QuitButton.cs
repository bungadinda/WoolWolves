using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Fungsi ini akan dipanggil ketika tombol Quit ditekan
    public void Quit()
    {
        // Jika sedang dalam editor, maka kita hanya berhenti play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Jika di build, maka keluar dari aplikasi
            Application.Quit();
#endif
    }
}
