using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : IEnemyState
{
    private EnemyAI enemy;
    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.StopFootStep();
        Transition.Ins.FadeIn();
        Debug.Log("Anda diserang dan kalah");
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
