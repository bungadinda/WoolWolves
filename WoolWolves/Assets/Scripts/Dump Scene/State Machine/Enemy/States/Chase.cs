public class Chase : IEnemyState
{
    private EnemyAI enemy;

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.chaseSpeed;
    }

    public void Execute()
    {
        if (!enemy.PlayerInView())
        {
            enemy.TransitionToState(new Patrol());
        }
        else if (enemy.PlayerInAttackRange())
        {
            enemy.TransitionToState(new Attack());
        }
        else
        {
            enemy.agent.destination = enemy.player.position;
        }
    }

    public void Exit()
    {
    }
}
