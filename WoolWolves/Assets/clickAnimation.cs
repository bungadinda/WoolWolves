using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickAnimation : MonoBehaviour
{
    private Animator animatorClick;
    // Start is called before the first frame update
    void Start()
    {
        animatorClick = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAnimation()
    {
        animatorClick.SetTrigger("clicked");
    }

    public void LevelClicked(string nameScene)
    {
        StartCoroutine(StartGame(nameScene));
    }

    IEnumerator StartGame(string nameScene)
    {
        animatorClick.Play("button-clicked");
        yield return new WaitForSeconds(1f);
        SceneController.Instance.LoadToScene(nameScene);
    }
}

