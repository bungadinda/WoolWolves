using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    public Animator animator;
    public string animationTrigger = "Intro";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        // Time.timeScale = 1;
        // Memulai animasi orang jatuh saat tombol ditekan
        animator.SetTrigger(animationTrigger);
        // Debug.Log("ok");
        // yield return new WaitForSecondsRealtime(14f);

        // Ubah Time.timeScale menjadi 1 untuk memulai game
        // Time.timeScale = 1;
        // Mulai Coroutine untuk menunggu hingga animasi selesai
        
        // Debug.Log("ok");
    }

    public void WaitForAnimationToEnd()
    {   
        // Setelah animasi selesai, ubah timescale menjadi 1 agar game dimulai
        Time.timeScale = 1;
    }
}
