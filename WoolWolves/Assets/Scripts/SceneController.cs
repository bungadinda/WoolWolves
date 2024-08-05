using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private Animator fadeAnim;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadToScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithAnimation(sceneName));
    }

    private IEnumerator LoadSceneWithAnimation(string nameScene)
    {
        Time.timeScale = 1;
        fadeAnim.SetTrigger("load");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nameScene);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        Time.timeScale = 1;
        fadeAnim.SetTrigger("load");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
