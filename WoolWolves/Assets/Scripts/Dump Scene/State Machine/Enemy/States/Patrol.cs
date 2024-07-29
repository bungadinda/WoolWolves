public class Patrol : IEnemyState
{
    private EnemyAI enemy;
    private int currentPatrolIndex;

    public void Enter(EnemyAI enemy)
    {
        this.enemy = enemy;
        enemy.agent.speed = enemy.patrolSpeed;
        currentPatrolIndex = 0;
        MoveToNextPatrolPoint();
    }

    public void Execute()
    {
        if (enemy.PlayerInView())
        {
            enemy.TransitionToState(new Chase());
        }
        else if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }
    }

    public void Exit()
    {
    }

    private void MoveToNextPatrolPoint()
    {
        if (enemy.patrolPoints.Length == 0)
            return;

        enemy.agent.destination = enemy.patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % enemy.patrolPoints.Length;
    }
}
