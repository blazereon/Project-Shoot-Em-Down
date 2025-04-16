public abstract class BaseGenericEnemy
{
    public abstract void EnterState(ManagerGenericEnemy genericEnemy);
    public abstract void UpdateState(ManagerGenericEnemy genericEnemy);
    public abstract void FixedUpdateState(ManagerGenericEnemy genericEnemy) ;
}