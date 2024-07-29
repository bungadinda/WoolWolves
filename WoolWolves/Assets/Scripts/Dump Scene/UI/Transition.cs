using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, timer / fadeDuration);
            yield return null;
        }

        fadeImage.color = Color.black;
        SceneManager.LoadScene("GameOver"); // Pindah ke scene GameOver setelah fade in
    }
}
