using JetBrains.Annotations;
using UnityEngine;

public class FragileMark : Effect
{
    Enemy EnemyInstance;
    Coroutine durationCouroutine;

    public FragileMark(Enemy enemy, float duration)
    {
        EnemyInstance = enemy;
        Duration = duration;
    }
    public override void OnEffectStart()
    {
        if (EnemyInstance == null)
        {
            EnemyInstance = (Enemy)EntityHolder;
        }
        Debug.Log("Fragile mark applied");
        EnemyInstance.AddEffect(this);
        EnemyInstance.OnConsumeMark += ConsumeMark;
        durationCouroutine = CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }

    public override void OnEffectEnd()
    {
        Debug.Log("Fragile mark end");
        EnemyInstance.RemoveEffect(this);
        EnemyInstance.OnConsumeMark -= ConsumeMark;
        CoroutineHandler.Instance.StopCoroutine(durationCouroutine);
    }

    public void ConsumeMark()
    {
        OnEffectEnd();
        EnemyInstance.TakeDamage(EnemyInstance.gameObject, DamageType.Melee, 10, 20, false);
        CoroutineHandler.Instance.StopCoroutine(durationCouroutine);
        Debug.Log("Fragile mark consumed");
    }

    public override Effect Clone()
    {
        return new FragileMark(null, Duration);
    }
}