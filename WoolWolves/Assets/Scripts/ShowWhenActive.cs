using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWhenActive : MonoBehaviour
{
    public GameObject ObjectA; //GameObjek yang ingin ditampilkan
    public GameObject ObjectB1;  //GameObject yang dipantau
    public GameObject ObjectB2;
    public GameObject ObjectB3;
    public GameObject ObjectB4;


    // Update is called once per frame
    private void Update()
    {
        if (ObjectB1.activeSelf || ObjectB2.activeSelf || ObjectB3.activeSelf || ObjectB4.activeSelf)  //cek apakah GameObject B aktif
        {
            ObjectA.SetActive(false);
        }
        else
        {
            ObjectA.SetActive(true);
        }
    }
}
