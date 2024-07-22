using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSkills : PlayerSkills, ISkillsHandler
{

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        UsingSkills();
    }

    public void UsingSkills()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) MakeSound();
        if(Input.GetKeyDown(KeyCode.Alpha2)) EatGrass();
    }

    void MakeSound()
    {
        Debug.Log("Mbeeee!");
    }

    void EatGrass()
    {
        Debug.Log("nyam nyam!");
    }
}
