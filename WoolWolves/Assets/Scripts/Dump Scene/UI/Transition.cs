using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private GameObject fadeObject;
    public static Transition Ins;

    void Awake() {
        Ins = this;
    }

    public void FadeOut() {
        Animator animator = fadeObject.GetComponent<Animator>();
        animator.SetBool("isIn", false);
    }

    public void FadeIn() {
        Animator animator = fadeObject.GetComponent<Animator>();
        animator.SetBool("isIn", true);
    }
}
