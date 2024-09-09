using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Referensi ke UI Pause Menu
    private bool isPaused = false; // Status apakah permainan sedang pause atau tidak

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape Key Pressed");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Sembunyikan menu pause
        Time.timeScale = 1f; // Resume permainan dengan mengatur time scale ke 1
        isPaused = false; // Set status pause ke false
        Debug.Log("Game Resume");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Tampilkan menu pause
        Time.timeScale = 0f; // Hentikan permainan dengan mengatur time scale ke 0
        isPaused = true; // Set status pause ke true
        Debug.Log("Game Paused");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume permainan jika kembali ke menu utama
        SceneManager.LoadScene("MainMenu"); // Ganti "MainMenu" dengan nama scene menu utama
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop permainan di editor
#else
        Application.Quit(); // Keluar dari permainan saat build
#endif
    }
}
