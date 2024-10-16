using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameplay : MonoBehaviour
{
    [Header("UI Gameplay")]
    public TextMeshProUGUI sheepCounterText;
    public TextMeshProUGUI timerText;
    private int sheepEaten = 0;
    // variable gameplay
    public int targetSheep = 10; // Jumlah domba yang harus dimakan
    public float gameDuration = 60.0f; // Durasi permainan dalam detik
    private float timer; // Waktu tersisa dalam permainan
    private bool isGameOver = false;
    [Header("Screens")]
    [SerializeField] private GameObject bgLose;
    [SerializeField] private GameObject bgwin;

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
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateSheepCounter()
    {
        sheepCounterText.text = sheepEaten + "/" + targetSheep;
    }

    public void EatSheep()
    {
        sheepEaten += 1; // Tambah 1 ke sheepEaten
        Debug.Log("Sheep eaten: " + sheepEaten);
        UpdateSheepCounter();

        if (sheepEaten >= targetSheep)
        {
            WinGame();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("sheep"))
        {
            EatSheep();
            Destroy(collision.gameObject); // Hapus domba setelah dimakan
        }
    }


    public void WinGame()
    {
        // Implementasi kemenangan permainan
        // Misalnya, pindah ke scene YouWin
        // SceneManager.LoadScene("YouWin");
        Time.timeScale = 0;
        bgwin.SetActive(true);
    }

    public void GameOver()
    {
        // Implementasi game over
        isGameOver = true;
        Time.timeScale = 0;
        bgLose.SetActive(true);
    }
}
