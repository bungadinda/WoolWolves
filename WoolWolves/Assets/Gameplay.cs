using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameplay : MonoBehaviour
{
    public TextMeshProUGUI sheepCounterText;
    public TextMeshProUGUI timerText;
    private int sheepEaten = 0;
    public int targetSheep = 10; // Jumlah domba yang harus dimakan
    public float gameDuration = 60.0f; // Durasi permainan dalam detik
    private float timer; // Waktu tersisa dalam permainan
    private bool isGameOver = false;

    void Start()
    {
        timer = gameDuration;
        UpdateTimerUI();
        UpdateSheepCounter();
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    void UpdateSheepCounter()
    {
        sheepCounterText.text = "Sheep Eaten: " + sheepEaten;
    }

    public void EatSheep()
    {
        sheepEaten++;
        UpdateSheepCounter();

        if (sheepEaten >= targetSheep)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        // Implementasi kemenangan permainan
        Debug.Log("You Win!");
        // Misalnya, pindah ke scene YouWin
        SceneManager.LoadScene("YouWin");
    }

    void GameOver()
    {
        // Implementasi game over
        Debug.Log("Game Over!");
        isGameOver = true;
        // Misalnya, pindah ke scene GameOver
        SceneManager.LoadScene("GameOver");
    }
}
