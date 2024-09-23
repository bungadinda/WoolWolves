using System.Collections; // Ini diperlukan untuk IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // Ini diperlukan untuk SceneManager

public class clickAnimation : MonoBehaviour
{
    private Animator animatorClick;

    void Start()
    {
        animatorClick = GetComponent<Animator>();
        animatorClick.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void LevelClicked(string sceneName)
    {
        StartCoroutine(StartGame(sceneName)); // Nama scene diterima sebagai parameter
    }

    IEnumerator StartGame(string sceneName)
    {
        animatorClick.Play("button-clicked");
        yield return new WaitForSecondsRealtime(1f);

        // Pastikan nama scene yang kamu masukkan valid
        SceneManager.LoadScene(sceneName); // Pindah ke scene baru
    }
}