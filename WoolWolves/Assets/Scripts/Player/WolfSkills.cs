using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSkills : PlayerSkills, ISkillsHandler
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        UsingSkills();
    }

    public void UsingSkills()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) MakeSound();
        if (Input.GetKeyDown(KeyCode.Return)) EatSheep();

        // Tambahkan pemanggilan skill transformasi
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerController.TransformToSphere();
        }
    }

    private void MakeSound()
    {
        Debug.Log("Auuuuuuuu!");
    }

    private void EatSheep()
    {
        Debug.Log("sksksskskskskkssk");
    }
}
