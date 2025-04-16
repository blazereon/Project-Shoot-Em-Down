using UnityEngine;
public class GenericEnemyAttackState : GenericEnemyBaseState
{
    public override void EnterState(GenericEnemyManager genericEnemy)
    {

    }

    public override void UpdateState(GenericEnemyManager genericEnemy)
    {
        genericEnemy.StartCoroutine(genericEnemy.LockOnPlayer());

        
    }

    public override void FixedUpdateState(GenericEnemyManager genericEnemy)
    {
        if (genericEnemy.facing == Enemy.EnemyFacing.Left)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.left.x * genericEnemy.ChasingSpeed * Time.fixedDeltaTime;
        }
        else if (genericEnemy.facing == Enemy.EnemyFacing.Right)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.right.x * genericEnemy.ChasingSpeed * Time.fixedDeltaTime;
        }
    }
}