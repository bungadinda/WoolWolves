using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public Animator animator; // Reference ke Animator yang menjalankan animasi
    public string nextSceneName; // Nama scene yang ingin dituju setelah animasi

    // Method ini akan dipanggil ketika animasi selesai
    void Update()
    {
        // Cek apakah tombol Esc ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipCutscene();
        }
    }

    // Method ini akan dipanggil ketika animasi selesai
    public void OnAnimationEnd()
    {
        LoadNextScene();
    }

    // Method untuk skip cutscene
    public void SkipCutscene()
    {
        LoadNextScene(); // Pindah ke scene berikutnya saat skip
    }

    // Method untuk memuat scene berikutnya
    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
