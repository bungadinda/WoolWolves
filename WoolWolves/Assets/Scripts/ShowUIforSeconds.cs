using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowUIforSeconds : MonoBehaviour
{
    public GameObject uiElement; //UI yang akan ditampilkan
    public float displayPlayTime  = 2f; //Waktu menampilkan UI yang dimaksud selama 2 detik
    public Button buttonPress;

    private void Start()
    {
        buttonPress.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        StartCoroutine(ShowUIforSecond());
    }

    IEnumerator ShowUIforSecond()
    {
        uiElement.SetActive(true);  
        yield return new WaitForSeconds(displayPlayTime);
        uiElement.SetActive(false);
    }
}
