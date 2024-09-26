using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : MonoBehaviour
{
    public Animator introAnimator;  // Referensi ke Animator yang menjalankan animasi intro
    private bool isGameStarted = false;

    void Start()
    {
        // Pause game saat mulai
        Time.timeScale = 0;
        // Jalankan animasi intro di Canvas
        introAnimator.Play("IntroAnimation");
    }

    // Fungsi ini dipanggil di akhir animasi menggunakan Animation Event
    public void OnIntroAnimationEnd()
    {
        // Setelah animasi selesai, game dimulai
        Time.timeScale = 1;  // Lanjutkan waktu permainan
        isGameStarted = true;
    }
}
