using JetBrains.Annotations;
using UnityEngine;

public class FragileMark : Effect
{
    Enemy EnemyInstance;

    public FragileMark(Enemy enemy, float duration)
    {
        EnemyInstance = enemy;
        Duration = duration;
    }
    public override void OnEffectStart()
    {
        Debug.Log("Fragile mark applied");
        EnemyInstance.OnConsumeMark += ConsumeMark;
        CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }

    public override void OnEffectEnd()
    {
        Debug.Log("Fragile mark end");
        EnemyInstance.OnConsumeMark -= ConsumeMark;
    }

    public void ConsumeMark()
    {
        OnEffectEnd();
        EnemyInstance.TakeDamage(EnemyInstance.gameObject, DamageType.Melee, 10, 20, false);
        Debug.Log("Fragile mark consumed");
    }
}