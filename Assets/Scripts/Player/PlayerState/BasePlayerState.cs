using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void EnterState(ManagerPlayerState player);
    public abstract void UpdateState(ManagerPlayerState player);
    public abstract void FixedUpdateState(ManagerPlayerState player);
    public abstract void OnCollisionEnter2DState(Collision2D collision, ManagerPlayerState player);
    public abstract void OnCollisionExit2DState(Collision2D collision, ManagerPlayerState player);
}