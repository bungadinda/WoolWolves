using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    protected static bool isWolf = false;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private GameObject sheepPrefab;
    

    // Update is called once per frame
    public virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3)) Transforming();
    }
    
    public void Transforming()
    {
        isWolf = !isWolf;
        if(isWolf)
        {
            wolfPrefab.SetActive(true);
            sheepPrefab.SetActive(false);
            Debug.Log("Menjadi Wolf");
        }
        else
        {
            
            wolfPrefab.SetActive(false);
            sheepPrefab.SetActive(true);
            Debug.Log("Menjadi mbe");
        }
    } 
}
