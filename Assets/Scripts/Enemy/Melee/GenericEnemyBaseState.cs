public abstract class GenericEnemyBaseState
{
    public abstract void EnterState(GenericEnemyManager genericEnemy);
    public abstract void UpdateState(GenericEnemyManager genericEnemy);
    public abstract void FixedUpdateState(GenericEnemyManager genericEnemy) ;
}