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
        fadeAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        fadeAnim.SetTrigger("load");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(nameScene);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        
        
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        fadeAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        fadeAnim.SetTrigger("load");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
