using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    public float alarmRadius = 10f; // radius alarmnya

    public void TriggerAlarm(Vector3 deathLoc)
    {
        // referensi ke enemy buat melakukan state investigate
        EnemyAI enemyAI = FindObjectOfType<EnemyAI>();
        enemyAI.InvestigateLocation(deathLoc);

        // cari semua object domba
        GameObject[] allSheep = GameObject.FindGameObjectsWithTag("sheep");
        foreach(GameObject sheep in allSheep)
        {
            // menghitung jarak domba ke lokasi kematian
            float distanceToSheep = Vector3.Distance(transform.position, deathLoc);
            // jika domba berada dalam radius alarm
            if(distanceToSheep <= alarmRadius)
            {
                // referensi ke domba
                AISheep aISheep = sheep.GetComponent<AISheep>();
                // jalankan function runaway from death location
                aISheep.RunAway(deathLoc, 10f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
