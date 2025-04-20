public abstract class BasePlayerState
{
    public abstract void EnterState(ManagerPlayerState player);
    public abstract void UpdateState(ManagerPlayerState player);
    public abstract void FixedUpdateState(ManagerPlayerState player);
}