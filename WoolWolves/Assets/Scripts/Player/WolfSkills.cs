using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSkills : PlayerSkills, ISkillsHandler
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
        if(Input.GetKeyDown(KeyCode.Return)) EatSheep();
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
