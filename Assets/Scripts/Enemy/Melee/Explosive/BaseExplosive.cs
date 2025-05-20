using UnityEngine;

public abstract class BaseExplosive
{
    public abstract void EnterState(ManagerExplosive enemy);

    public abstract void UpdateState(ManagerExplosive enemy);
    
    public abstract void FixedUpdateState(ManagerExplosive enemy);
}
